using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.FaqManager.Admin.Factories;
using Nop.Plugin.Misc.FaqManager.Admin.Models;
using Nop.Services.Catalog;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.FaqManager.Admin.Controllers;


[AuthorizeAdmin]
[Area(AreaNames.ADMIN)]
[AutoValidateAntiforgeryToken]
public class FaqGroupController : BasePluginController
{
    #region Fields

    private readonly FaqGroupModelFactory _faqGroupModelFactory;
    private readonly IProductService _productService;

    #endregion

    #region Ctor

    public FaqGroupController(
        FaqGroupModelFactory faqGroupModelFactory,
        IProductService productService)
    {
        _faqGroupModelFactory = faqGroupModelFactory;
        _productService = productService;
    }

    #endregion

    #region List

    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_GROUPS_VIEW)]
    public virtual async Task<IActionResult> List()
    {
        var model = await _faqGroupModelFactory
            .PrepareFaqGroupSearchModelAsync(
                new FaqGroupSearchModel());

        return View("~/Plugins/Misc.FaqManager/Admin/Views/FaqGroup/List.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_GROUPS_VIEW)]
    public virtual async Task<IActionResult> List(FaqGroupSearchModel searchModel)
    {
        var model = await _faqGroupModelFactory
            .PrepareFaqGroupListModelAsync(searchModel);

        return Json(model);
    }

    #endregion

    #region Product search popup

    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_GROUPS_VIEW)]
    public virtual async Task<IActionResult> ProductSearchPopup()
    {
        var model = await _faqGroupModelFactory
            .PrepareFaqGroupProductSearchModelAsync(new FaqGroupProductSearchModel());

        return View("~/Plugins/Misc.FaqManager/Admin/Views/FaqGroup/ProductSearchPopup.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_GROUPS_VIEW)]
    public virtual async Task<IActionResult> ProductSearchPopupList(FaqGroupProductSearchModel searchModel)
    {
        var model = await _faqGroupModelFactory
            .PrepareFaqGroupProductListModelAsync(searchModel);

        return Json(model);
    }

    [HttpPost]
    [FormValueRequired("save")]
    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_GROUPS_VIEW)]
    public virtual async Task<IActionResult> ProductSearchPopup([Bind(Prefix = nameof(AddProductToFaqGroupModel))] AddProductToFaqGroupModel model)
    {
        var product = await _productService.GetProductByIdAsync(model.AssociatedToProductId);
        if (product == null)
            return Content("Cannot load a product");

        ViewBag.RefreshPage = true;
        ViewBag.productId = product.Id;
        ViewBag.productName = product.Name;

        var searchModel = await _faqGroupModelFactory
            .PrepareFaqGroupProductSearchModelAsync(new FaqGroupProductSearchModel());

        return View("~/Plugins/Misc.FaqManager/Admin/Views/FaqGroup/ProductSearchPopup.cshtml", searchModel);
    }

    #endregion
}