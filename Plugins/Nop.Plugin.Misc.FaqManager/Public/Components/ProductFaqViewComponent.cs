using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Misc.FaqManager.Public.Components;

/// <summary>
/// Represents a view component for displaying faqs on the product details page
/// </summary>
public class ProductFaqViewComponent : NopViewComponent
{
    #region Methods

    /// <summary>
    /// Invoke the widget view component
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the view component result
    /// </returns>
    public async Task<IViewComponentResult> InvokeAsync()
    {
        // var model = await _faqModelFactory.PrepareProductFaqModelAsync();
        return await ViewAsync("~/Plugins/Misc.FaqManager/Public/Views/Components/ProductFaq.cshtml");
    }

    #endregion
}
