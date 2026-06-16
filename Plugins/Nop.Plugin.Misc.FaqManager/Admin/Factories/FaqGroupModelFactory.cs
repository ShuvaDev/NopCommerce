using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Misc.FaqManager.Admin.Models;
using Nop.Plugin.Misc.FaqManager.Domain;
using Nop.Plugin.Misc.FaqManager.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Seo;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Models.Extensions;
using Nop.Web.Areas.Admin.Factories;

namespace Nop.Plugin.Misc.FaqManager.Admin.Factories;

public class FaqGroupModelFactory
{
    #region Fields

    private readonly IFaqGroupService _faqGroupService;
    private readonly IProductService _productService;
    private readonly ILanguageService _languageService;
    private readonly ILocalizedEntityService _localizedEntityService;
    private readonly IBaseAdminModelFactory _baseAdminModelFactory;
    private readonly IUrlRecordService _urlRecordService;

    #endregion

    #region Ctor

    public FaqGroupModelFactory(
        IFaqGroupService faqGroupService,
        IProductService productService,
        ILanguageService languageService,
        ILocalizedEntityService localizedEntityService,
        IBaseAdminModelFactory baseAdminModelFactory,
        IUrlRecordService urlRecordService)
    {
        _faqGroupService = faqGroupService;
        _productService = productService;
        _languageService = languageService;
        _localizedEntityService = localizedEntityService;
        _baseAdminModelFactory = baseAdminModelFactory;
        _urlRecordService = urlRecordService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Prepare FAQ group search model
    /// </summary>
    /// <param name="searchModel">Search model</param>
    /// <returns>FAQ group search model</returns>
    public virtual async Task<FaqGroupSearchModel> PrepareFaqGroupSearchModelAsync(
        FaqGroupSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        searchModel.SetGridPageSize();

        searchModel.AvailablePublishedOptions.Add(new SelectListItem { Text = "All", Value = "" });
        searchModel.AvailablePublishedOptions.Add(new SelectListItem { Text = "Published only", Value = "true" });
        searchModel.AvailablePublishedOptions.Add(new SelectListItem { Text = "Unpublished only", Value = "false" });

        if (searchModel.SearchProductId > 0)
        {
            var product = await _productService.GetProductByIdAsync(searchModel.SearchProductId);
            searchModel.SearchProductName = product?.Name;
        }

        return searchModel;
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

    public virtual async Task<FaqGroupProductSearchModel> PrepareFaqGroupProductSearchModelAsync(
        FaqGroupProductSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        searchModel.IsLoggedInAsVendor = false;

        await _baseAdminModelFactory.PrepareCategoriesAsync(searchModel.AvailableCategories);
        await _baseAdminModelFactory.PrepareManufacturersAsync(searchModel.AvailableManufacturers);
        await _baseAdminModelFactory.PrepareStoresAsync(searchModel.AvailableStores);
        await _baseAdminModelFactory.PrepareVendorsAsync(searchModel.AvailableVendors);
        await _baseAdminModelFactory.PrepareProductTypesAsync(searchModel.AvailableProductTypes);

        searchModel.SetPopupGridPageSize();

        return searchModel;
    }

    public virtual async Task<FaqGroupProductListModel> PrepareFaqGroupProductListModelAsync(
        FaqGroupProductSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        var products = await _productService.SearchProductsAsync(
            showHidden: true,
            categoryIds: new List<int> { searchModel.SearchCategoryId },
            manufacturerIds: new List<int> { searchModel.SearchManufacturerId },
            storeId: searchModel.SearchStoreId,
            vendorId: searchModel.SearchVendorId,
            productType: searchModel.SearchProductTypeId > 0
                ? (ProductType?)searchModel.SearchProductTypeId
                : null,
            keywords: searchModel.SearchProductName,
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize);

        var model = await new FaqGroupProductListModel().PrepareToGridAsync(
            searchModel,
            products,
            () => products.SelectAwait(async product =>
            {
                var productModel = product.ToModel<ProductModel>();

                productModel.SeName = await _urlRecordService
                    .GetSeNameAsync(product, 0, true, false);

                return productModel;
            }));

        return model;
    }

    #endregion
}

