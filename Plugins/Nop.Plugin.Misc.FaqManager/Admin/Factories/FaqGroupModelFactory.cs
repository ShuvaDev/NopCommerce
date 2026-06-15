using Nop.Plugin.Misc.FaqManager.Admin.Models;
using Nop.Plugin.Misc.FaqManager.Domain;
using Nop.Plugin.Misc.FaqManager.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.FaqManager.Admin.Factories;

/// <summary>
/// Represents a FAQ group model factory
/// </summary>
public class FaqGroupModelFactory
{
    #region Fields

    private readonly IFaqGroupService _faqGroupService;
    private readonly IProductService _productService;
    private readonly ILanguageService _languageService;
    private readonly ILocalizedEntityService _localizedEntityService;

    #endregion

    #region Ctor

    public FaqGroupModelFactory(
        IFaqGroupService faqGroupService,
        IProductService productService,
        ILanguageService languageService,
        ILocalizedEntityService localizedEntityService)
    {
        _faqGroupService = faqGroupService;
        _productService = productService;
        _languageService = languageService;
        _localizedEntityService = localizedEntityService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Prepare FAQ group search model
    /// </summary>
    /// <param name="searchModel">Search model</param>
    /// <returns>FAQ group search model</returns>
    public virtual Task<FaqGroupSearchModel> PrepareFaqGroupSearchModelAsync(
        FaqGroupSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }

    /// <summary>
    /// Prepare FAQ group list model
    /// </summary>
    /// <param name="searchModel">Search model</param>
    /// <returns>FAQ group list model</returns>
    public virtual async Task<FaqGroupListModel> PrepareFaqGroupListModelAsync(
        FaqGroupSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        var faqGroups = await _faqGroupService.GetAllFaqGroupsAsync(
            name: searchModel.SearchName,
            productId: searchModel.SearchProductId,
            published: searchModel.SearchPublished,
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize);

        var model = await new FaqGroupListModel().PrepareToGridAsync(
            searchModel,
            faqGroups,
            () => faqGroups.SelectAwait(async faqGroup =>
            {
                var product = await _productService.GetProductByIdAsync(
                    faqGroup.ProductId);

                return new FaqGroupModel
                {
                    Id = faqGroup.Id,
                    Name = faqGroup.Name,
                    ProductId = faqGroup.ProductId,
                    ProductName = product?.Name,
                    Published = faqGroup.Published,
                    DisplayOrder = faqGroup.DisplayOrder
                };
            }));

        return model;
    }

    /// <summary>
    /// Prepare FAQ group model
    /// </summary>
    /// <param name="model">Model</param>
    /// <param name="faqGroup">FAQ group</param>
    /// <param name="excludeProperties">Whether to exclude model properties</param>
    /// <returns>FAQ group model</returns>
    public virtual async Task<FaqGroupModel> PrepareFaqGroupModelAsync(
        FaqGroupModel model,
        FaqGroup faqGroup,
        bool excludeProperties = false)
    {
        if (faqGroup != null)
        {
            model ??= new FaqGroupModel();

            model.Id = faqGroup.Id;
            model.Name = faqGroup.Name;
            model.ProductId = faqGroup.ProductId;
            model.Published = faqGroup.Published;
            model.DisplayOrder = faqGroup.DisplayOrder;

            var product = await _productService.GetProductByIdAsync(
                faqGroup.ProductId);

            model.ProductName = product?.Name;

            if (!excludeProperties)
            {
                var languages = await _languageService.GetAllLanguagesAsync();

                foreach (var language in languages)
                {
                    model.Locales.Add(new FaqGroupLocalizedModel
                    {
                        LanguageId = language.Id,
                        Name = await _localizedEntityService.GetLocalizedValueAsync(
                            language.Id,
                            model.Id,
                            nameof(FaqGroup),
                            nameof(FaqGroup.Name))
                    });
                }
            }
        }

        return model;
    }

    #endregion
}

