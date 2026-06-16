using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.FaqManager.Admin.Factories;
using Nop.Plugin.Misc.FaqManager.Admin.Models;
using Nop.Plugin.Misc.FaqManager.Domain;
using Nop.Plugin.Misc.FaqManager.Services;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.FaqManager.Admin.Controllers;


[AuthorizeAdmin]
[Area(AreaNames.ADMIN)]
[AutoValidateAntiforgeryToken]
public class FaqGroupController : BasePluginController
{
    #region Fields

    private readonly FaqGroupModelFactory _faqGroupModelFactory;
    private readonly IFaqGroupService _faqGroupService;
    private readonly IProductService _productService;
    private readonly ILocalizedEntityService _localizedEntityService;
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;
    private readonly ISettingService _settingService;
    private readonly IStoreContext _storeContext;

    #endregion

    #region Ctor

    public FaqGroupController(
        FaqGroupModelFactory faqGroupModelFactory,
        IFaqGroupService faqGroupService,
        IProductService productService,
        ILocalizedEntityService localizedEntityService,
        ILocalizationService localizationService,
        INotificationService notificationService,
        ISettingService settingService,
        IStoreContext storeContext)
    {
        _faqGroupModelFactory = faqGroupModelFactory;
        _faqGroupService = faqGroupService;
        _productService = productService;
        _localizedEntityService = localizedEntityService;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _settingService = settingService;
        _storeContext = storeContext;
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

    #region Create / Edit / Delete

    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_GROUPS_MANAGE)]
    public virtual async Task<IActionResult> Create()
    {
        var model = await _faqGroupModelFactory
            .PrepareFaqGroupModelAsync(new FaqGroupModel(), null);

        return View("~/Plugins/Misc.FaqManager/Admin/Views/FaqGroup/Create.cshtml", model);
    }

    [HttpPost]
    [ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_GROUPS_MANAGE)]
    public virtual async Task<IActionResult> Create(FaqGroupModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var existingGroup = await _faqGroupService
                .GetPublishedFaqGroupByProductIdAsync(model.ProductId);
            if (existingGroup != null)
            {
                ModelState.AddModelError(string.Empty,
                    await _localizationService.GetResourceAsync(
                        "Plugins.Misc.FaqManager.Groups.Product.AlreadyAssigned"));
            }
        }

        if (ModelState.IsValid)
        {
            var faqGroup = new FaqGroup
            {
                Name = model.Name,
                ProductId = model.ProductId,
                Published = model.Published,
                DisplayOrder = model.DisplayOrder
            };

            await _faqGroupService.InsertFaqGroupAsync(faqGroup);

            await UpdateFaqGroupLocalesAsync(faqGroup, model);

            _notificationService.SuccessNotification(
                await _localizationService.GetResourceAsync("Plugins.Misc.FaqManager.Groups.Added"));

            return continueEditing
                ? RedirectToAction("Edit", new { id = faqGroup.Id })
                : RedirectToAction("List");
        }

        model = await _faqGroupModelFactory.PrepareFaqGroupModelAsync(model, null, true);

        return View("~/Plugins/Misc.FaqManager/Admin/Views/FaqGroup/Create.cshtml", model);
    }

    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_GROUPS_VIEW)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        var faqGroup = await _faqGroupService.GetFaqGroupByIdAsync(id);
        if (faqGroup == null)
            return RedirectToAction("List");

        var model = await _faqGroupModelFactory
            .PrepareFaqGroupModelAsync(null, faqGroup);

        return View("~/Plugins/Misc.FaqManager/Admin/Views/FaqGroup/Edit.cshtml", model);
    }

    [HttpPost]
    [ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_GROUPS_MANAGE)]
    public virtual async Task<IActionResult> Edit(FaqGroupModel model, bool continueEditing)
    {
        var faqGroup = await _faqGroupService.GetFaqGroupByIdAsync(model.Id);
        if (faqGroup == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            var existingGroup = await _faqGroupService
                .GetFaqGroupsByProductIdAsync(model.ProductId, true);
            if (existingGroup.Any(g => g.Id != model.Id))
            {
                ModelState.AddModelError(string.Empty,
                    await _localizationService.GetResourceAsync(
                        "Plugins.Misc.FaqManager.Groups.Product.AlreadyAssigned"));
            }
        }

        if (ModelState.IsValid)
        {
            faqGroup.Name = model.Name;
            faqGroup.ProductId = model.ProductId;
            faqGroup.Published = model.Published;
            faqGroup.DisplayOrder = model.DisplayOrder;

            await _faqGroupService.UpdateFaqGroupAsync(faqGroup);

            await UpdateFaqGroupLocalesAsync(faqGroup, model);

            _notificationService.SuccessNotification(
                await _localizationService.GetResourceAsync("Plugins.Misc.FaqManager.Groups.Updated"));

            return continueEditing
                ? RedirectToAction("Edit", new { id = faqGroup.Id })
                : RedirectToAction("List");
        }

        model = await _faqGroupModelFactory.PrepareFaqGroupModelAsync(model, faqGroup, true);

        return View("~/Plugins/Misc.FaqManager/Admin/Views/FaqGroup/Edit.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_GROUPS_MANAGE)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        var faqGroup = await _faqGroupService.GetFaqGroupByIdAsync(id);
        if (faqGroup == null)
            return RedirectToAction("List");

        try
        {
            await _faqGroupService.DeleteFaqGroupAsync(faqGroup);

            _notificationService.SuccessNotification(
                await _localizationService.GetResourceAsync("Plugins.Misc.FaqManager.Groups.Deleted"));
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
        }

        if (Request.Headers.XRequestedWith == "XMLHttpRequest")
            return new NullJsonResult();

        return RedirectToAction("List");
    }

    #endregion

    #region Configure

    [CheckPermission(StandardPermission.Configuration.MANAGE_PLUGINS)]
    public virtual async Task<IActionResult> Configure()
    {
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var settings = await _settingService.LoadSettingAsync<FaqManagerSettings>(storeScope);

        var model = new ConfigurationModel
        {
            ShowFaqCount = settings.ShowFaqCount
        };

        return View("~/Plugins/Misc.FaqManager/Admin/Views/FaqGroup/Configure.cshtml", model);
    }

    [HttpPost, ActionName("Configure")]
    [FormValueRequired("save")]
    [CheckPermission(StandardPermission.Configuration.MANAGE_PLUGINS)]
    public virtual async Task<IActionResult> Configure(ConfigurationModel model)
    {
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var settings = await _settingService.LoadSettingAsync<FaqManagerSettings>(storeScope);

        settings.ShowFaqCount = model.ShowFaqCount;

        await _settingService.SaveSettingAsync(settings, x => x.ShowFaqCount, storeScope);
        await _settingService.ClearCacheAsync();

        _notificationService.SuccessNotification(
            await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

        return RedirectToAction("Configure");
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

    #region Utilities

    private async Task UpdateFaqGroupLocalesAsync(FaqGroup faqGroup, FaqGroupModel model)
    {
        foreach (var locale in model.Locales)
        {
            await _localizedEntityService.SaveLocalizedValueAsync(
                faqGroup,
                x => x.Name,
                locale.Name,
                locale.LanguageId);
        }
    }

    #endregion
}