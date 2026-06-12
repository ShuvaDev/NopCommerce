using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Events;
using Nop.Data;
using Nop.Plugin.Misc.FaqManager.Domain;
using Nop.Services.Caching;

namespace Nop.Plugin.Misc.FaqManager.Services;

/// <summary>
/// Provides methods for CRUD operations and retrieval of FAQ groups.
/// </summary>
public class FaqGroupService : IFaqGroupService
{
    #region Fields

    private readonly IRepository<FaqGroup> _faqGroupRepository;
    private readonly IFaqItemService _faqItemService;
    private readonly IEventPublisher _eventPublisher;

    #endregion

    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="FaqGroupService"/> class.
    /// </summary>
    /// <param name="faqGroupRepository">FAQ group repository.</param>
    /// <param name="faqItemService">FAQ item service.</param>
    /// <param name="eventPublisher">Event publisher.</param>
    public FaqGroupService(
        IRepository<FaqGroup> faqGroupRepository,
        IFaqItemService faqItemService,
        IEventPublisher eventPublisher)
    {
        _faqGroupRepository = faqGroupRepository;
        _faqItemService = faqItemService;
        _eventPublisher = eventPublisher;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets a FAQ group by identifier.
    /// </summary>
    /// <param name="faqGroupId">FAQ group identifier.</param>
    /// <returns>
    /// The FAQ group if found; otherwise, <c>null</c>.
    /// </returns>
    public virtual async Task<FaqGroup?> GetFaqGroupByIdAsync(int faqGroupId)
    {
        if (faqGroupId <= 0)
            return null;

        return await _faqGroupRepository.GetByIdAsync(faqGroupId);
    }

    /// <summary>
    /// Gets FAQ groups.
    /// </summary>
    /// <param name="name">Name filter.</param>
    /// <param name="productId">Product identifier.</param>
    /// <param name="published">Published status filter.</param>
    /// <param name="pageIndex">Page index.</param>
    /// <param name="pageSize">Page size.</param>
    /// <returns>A paged list of FAQ groups.</returns>
    public virtual async Task<IPagedList<FaqGroup>> GetAllFaqGroupsAsync(
        string? name = null,
        int productId = 0,
        bool? published = null,
        int pageIndex = 0,
        int pageSize = int.MaxValue)
    {
        var query = _faqGroupRepository.Table;

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(x => x.Name.Contains(name));

        if (productId > 0)
            query = query.Where(x => x.ProductId == productId);

        if (published.HasValue)
            query = query.Where(x => x.Published == published.Value);

        query = query.OrderBy(x => x.DisplayOrder)
                     .ThenBy(x => x.Id);

        return await query.ToPagedListAsync(pageIndex, pageSize);
    }

    /// <summary>
    /// Gets FAQ groups associated with a product.
    /// </summary>
    /// <param name="productId">Product identifier.</param>
    /// <param name="showHidden">
    /// A value indicating whether unpublished groups should be included.
    /// </param>
    /// <returns>A list of FAQ groups.</returns>
    public virtual async Task<IList<FaqGroup>> GetFaqGroupsByProductIdAsync(
        int productId,
        bool showHidden = false)
    {
        if (productId <= 0)
            return [];

        var query = _faqGroupRepository.Table;

        query = query.Where(x => x.ProductId == productId);

        if (!showHidden)
            query = query.Where(x => x.Published);

        query = query.OrderBy(x => x.DisplayOrder);

        return await query.ToListAsync();
    }

    /// <summary>
    /// Gets the first published FAQ group associated with a product.
    /// </summary>
    /// <param name="productId">Product identifier.</param>
    /// <returns>
    /// A published FAQ group if found; otherwise, <c>null</c>.
    /// </returns>
    public virtual async Task<FaqGroup?> GetPublishedFaqGroupByProductIdAsync(
        int productId)
    {
        if (productId <= 0)
            return null;

        var query = _faqGroupRepository.Table;

        query = query.Where(x =>
            x.ProductId == productId &&
            x.Published);

        query = query.OrderBy(x => x.DisplayOrder);

        return await query.FirstOrDefaultAsync();
    }

    /// <summary>
    /// Inserts a FAQ group.
    /// </summary>
    /// <param name="faqGroup">FAQ group.</param>
    public virtual async Task InsertFaqGroupAsync(FaqGroup faqGroup)
    {
        ArgumentNullException.ThrowIfNull(faqGroup);

        await _faqGroupRepository.InsertAsync(faqGroup);

        await _eventPublisher.EntityInsertedAsync(faqGroup);
    }

    /// <summary>
    /// Updates a FAQ group.
    /// </summary>
    /// <param name="faqGroup">FAQ group.</param>
    public virtual async Task UpdateFaqGroupAsync(FaqGroup faqGroup)
    {
        ArgumentNullException.ThrowIfNull(faqGroup);

        await _faqGroupRepository.UpdateAsync(faqGroup);

        await _eventPublisher.EntityUpdatedAsync(faqGroup);
    }

    /// <summary>
    /// Deletes a FAQ group and all associated FAQ items.
    /// </summary>
    /// <param name="faqGroup">FAQ group.</param>
    public virtual async Task DeleteFaqGroupAsync(FaqGroup faqGroup)
    {
        ArgumentNullException.ThrowIfNull(faqGroup);

        var faqItems = await _faqItemService.GetFaqItemsByGroupIdAsync(
            faqGroup.Id,
            true);

        foreach (var faqItem in faqItems)
        {
            await _faqItemService.DeleteFaqItemAsync(faqItem);
        }

        await _faqGroupRepository.DeleteAsync(faqGroup);

        await _eventPublisher.EntityDeletedAsync(faqGroup);
    }

    #endregion
}
