using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.FaqManager.Public.Factories;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Misc.FaqManager.Public.Components;

/// <summary>
/// Represents a view component for displaying faqs on the product details page
/// </summary>
public class ProductFaqViewComponent : NopViewComponent
{
    #region Fields

    protected readonly ProductFaqModelFactory _productFaqModelFactory;

    #endregion

    #region Ctor

    public ProductFaqViewComponent(
        ProductFaqModelFactory productFaqModelFactory)
    {
        _productFaqModelFactory = productFaqModelFactory;
    }

    #endregion

    #region Methods

    public async Task<IViewComponentResult> InvokeAsync(
        string widgetZone,
        object additionalData)
    {
        if (additionalData is not ProductDetailsModel productDetailsModel)
            return Content(string.Empty);

        var model =
            await _productFaqModelFactory
                .PrepareProductFaqModelAsync(productDetailsModel.Id);

        if (model == null || !model.Items.Any())
            return Content(string.Empty);

        return await ViewAsync("~/Plugins/Misc.FaqManager/Public/Views/Components/ProductFaq.cshtml", model);
    }

    #endregion
}
