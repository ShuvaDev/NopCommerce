using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Events;
using Nop.Data;
using Nop.Plugin.Misc.FaqManager.Domain;

namespace Nop.Plugin.Misc.FaqManager.Services;

/// <summary>
/// Provides methods for CRUD operations and retrieval of FAQ items.
/// </summary>
public class FaqItemService : IFaqItemService
{
    #region Fields

    private readonly IRepository<FaqItem> _faqItemRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly IStaticCacheManager _staticCacheManager;
    #endregion

    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="FaqItemService"/> class.
    /// </summary>
    /// <param name="faqItemRepository">FAQ item repository.</param>
    /// <param name="eventPublisher">Event publisher.</param>
    /// <param name="staticCacheManager">Static cache manager.</param>
    public FaqItemService(IRepository<FaqItem> faqItemRepository,
        IEventPublisher eventPublisher,
        IStaticCacheManager staticCacheManager)
    {
        _faqItemRepository = faqItemRepository;
        _eventPublisher = eventPublisher;
        _staticCacheManager = staticCacheManager;
    }

    #endregion

    #region Methods

    public virtual async Task<IPagedList<FaqItem>> GetAllFaqItemsAsync(
        string question = null,
        int faqGroupId = 0,
        bool? published = null,
        int pageIndex = 0,
        int pageSize = int.MaxValue)
    {
        var query = _faqItemRepository.Table;

        if (!string.IsNullOrWhiteSpace(question))
            query = query.Where(x => x.Question.Contains(question));

        if (faqGroupId > 0)
            query = query.Where(x => x.FaqGroupId == faqGroupId);

        if (published.HasValue)
            query = query.Where(x => x.Published == published.Value);

        query = query.OrderBy(x => x.DisplayOrder)
                     .ThenBy(x => x.Id);

        return await query.ToPagedListAsync(pageIndex, pageSize);
    }

    /// <summary>
    /// Gets a FAQ item by identifier.
    /// </summary>
    /// <param name="faqItemId">FAQ item identifier.</param>
    /// <returns>
    /// The FAQ item if found; otherwise, null</c>.
    /// </returns>
    public virtual async Task<FaqItem?> GetFaqItemByIdAsync(int faqItemId)
    {
        if (faqItemId <= 0)
            return null;

        return await _faqItemRepository.GetByIdAsync(faqItemId);
    }

    /// <summary>
    /// Gets FAQ items belonging to the specified FAQ group.
    /// </summary>
    /// <param name="faqGroupId">FAQ group identifier.</param>
    /// <param name="pageIndex">Page index.</param>
    /// <param name="pageSize">Page size.</param>
    /// <returns>
    /// A paged list of FAQ items.
    /// </returns>
    public virtual async Task<IPagedList<FaqItem>> GetFaqItemsAsync(
        int faqGroupId,
        int pageIndex = 0,
        int pageSize = int.MaxValue)
    {
        var query = _faqItemRepository.Table;

        if (faqGroupId > 0)
            query = query.Where(x => x.FaqGroupId == faqGroupId);

        query = query.OrderBy(x => x.DisplayOrder)
                     .ThenBy(x => x.Id);

        return await query.ToPagedListAsync(pageIndex, pageSize);
    }

    /// <summary>
    /// Gets FAQ items belonging to the specified FAQ group.
    /// </summary>
    /// <param name="faqGroupId">FAQ group identifier.</param>
    /// <param name="showHidden">
    /// A value indicating whether unpublished items should be included.
    /// </param>
    /// <returns>
    /// A list of FAQ items.
    /// </returns>
    public virtual async Task<IList<FaqItem>> GetFaqItemsByGroupIdAsync(
        int faqGroupId,
        bool showHidden = false)
    {
        if (faqGroupId <= 0)
            return new List<FaqItem>();

        var query = _faqItemRepository.Table
            .Where(x => x.FaqGroupId == faqGroupId);

        if (!showHidden)
            query = query.Where(x => x.Published);

        query = query.OrderBy(x => x.DisplayOrder);

        return await query.ToListAsync();
    }

    /// <summary>
    /// Gets published FAQ items belonging to the specified FAQ group.
    /// </summary>
    /// <param name="faqGroupId">FAQ group identifier.</param>
    /// <returns>
    /// A list of published FAQ items.
    /// </returns>
    public virtual async Task<IList<FaqItem>> GetPublishedFaqItemsByGroupIdAsync(
        int faqGroupId)
    {
        if (faqGroupId <= 0)
            return new List<FaqItem>();

        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
            FaqManagerDefaults.FaqItemsByGroupIdCacheKey,
            faqGroupId);

        return await _staticCacheManager.GetAsync(cacheKey, async () =>
        {
            return await _faqItemRepository.Table
                .Where(x =>
                    x.FaqGroupId == faqGroupId &&
                    x.Published)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        });
    }

    /// <summary>
    /// Inserts a FAQ item.
    /// </summary>
    /// <param name="faqItem">FAQ item.</param>
    public virtual async Task InsertFaqItemAsync(FaqItem faqItem)
    {
        ArgumentNullException.ThrowIfNull(faqItem);

        await _faqItemRepository.InsertAsync(faqItem);

        await _eventPublisher.EntityInsertedAsync(faqItem);
    }

    /// <summary>
    /// Updates a FAQ item.
    /// </summary>
    /// <param name="faqItem">FAQ item.</param>
    public virtual async Task UpdateFaqItemAsync(FaqItem faqItem)
    {
        ArgumentNullException.ThrowIfNull(faqItem);

        await _faqItemRepository.UpdateAsync(faqItem);

        await _eventPublisher.EntityUpdatedAsync(faqItem);
    }

    /// <summary>
    /// Deletes a FAQ item.
    /// </summary>
    /// <param name="faqItem">FAQ item.</param>
    public virtual async Task DeleteFaqItemAsync(FaqItem faqItem)
    {
        ArgumentNullException.ThrowIfNull(faqItem);

        await _faqItemRepository.DeleteAsync(faqItem);

        await _eventPublisher.EntityDeletedAsync(faqItem);
    }

    #endregion
}