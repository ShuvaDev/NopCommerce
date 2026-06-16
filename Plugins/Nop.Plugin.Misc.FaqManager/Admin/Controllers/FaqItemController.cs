using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.FaqManager.Admin.Factories;
using Nop.Plugin.Misc.FaqManager.Admin.Models;
using Nop.Plugin.Misc.FaqManager.Domain;
using Nop.Plugin.Misc.FaqManager.Services;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.FaqManager.Admin.Controllers;

[AuthorizeAdmin]
[Area(AreaNames.ADMIN)]
[AutoValidateAntiforgeryToken]
public class FaqItemController : BasePluginController
{
    #region Fields

    private readonly FaqItemModelFactory _faqItemModelFactory;
    private readonly IFaqItemService _faqItemService;
    private readonly ILocalizedEntityService _localizedEntityService;
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;

    #endregion

    #region Ctor

    public FaqItemController(
        FaqItemModelFactory faqItemModelFactory,
        IFaqItemService faqItemService,
        ILocalizedEntityService localizedEntityService,
        ILocalizationService localizationService,
        INotificationService notificationService)
    {
        _faqItemModelFactory = faqItemModelFactory;
        _faqItemService = faqItemService;
        _localizedEntityService = localizedEntityService;
        _localizationService = localizationService;
        _notificationService = notificationService;
    }

    #endregion

    #region List

    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_ITEMS_VIEW)]
    public virtual async Task<IActionResult> List()
    {
        var model = await _faqItemModelFactory
            .PrepareFaqItemSearchModelAsync(new FaqItemSearchModel());

        return View("~/Plugins/Misc.FaqManager/Admin/Views/FaqItem/List.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_ITEMS_VIEW)]
    public virtual async Task<IActionResult> List(FaqItemSearchModel searchModel)
    {
        var model = await _faqItemModelFactory
            .PrepareFaqItemListModelAsync(searchModel);

        return Json(model);
    }

    #endregion

    #region Create / Edit / Delete

    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_ITEMS_MANAGE)]
    public virtual async Task<IActionResult> Create()
    {
        var model = await _faqItemModelFactory
            .PrepareFaqItemModelAsync(new FaqItemModel(), null);

        return View("~/Plugins/Misc.FaqManager/Admin/Views/FaqItem/Create.cshtml", model);
    }

    [HttpPost]
    [ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_ITEMS_MANAGE)]
    public virtual async Task<IActionResult> Create(FaqItemModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var faqItem = new FaqItem
            {
                FaqGroupId = model.FaqGroupId,
                Question = model.Question,
                Answer = model.Answer,
                Published = model.Published,
                DisplayOrder = model.DisplayOrder
            };

            await _faqItemService.InsertFaqItemAsync(faqItem);

            await UpdateFaqItemLocalesAsync(faqItem, model);

            _notificationService.SuccessNotification(
                await _localizationService.GetResourceAsync("Plugins.Misc.FaqManager.Items.Added"));

            return continueEditing
                ? RedirectToAction("Edit", new { id = faqItem.Id })
                : RedirectToAction("List");
        }

        model = await _faqItemModelFactory.PrepareFaqItemModelAsync(model, null, true);

        return View("~/Plugins/Misc.FaqManager/Admin/Views/FaqItem/Create.cshtml", model);
    }

    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_ITEMS_VIEW)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        var faqItem = await _faqItemService.GetFaqItemByIdAsync(id);
        if (faqItem == null)
            return RedirectToAction("List");

        var model = await _faqItemModelFactory
            .PrepareFaqItemModelAsync(null, faqItem);

        return View("~/Plugins/Misc.FaqManager/Admin/Views/FaqItem/Edit.cshtml", model);
    }

    [HttpPost]
    [ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_ITEMS_MANAGE)]
    public virtual async Task<IActionResult> Edit(FaqItemModel model, bool continueEditing)
    {
        var faqItem = await _faqItemService.GetFaqItemByIdAsync(model.Id);
        if (faqItem == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            faqItem.FaqGroupId = model.FaqGroupId;
            faqItem.Question = model.Question;
            faqItem.Answer = model.Answer;
            faqItem.Published = model.Published;
            faqItem.DisplayOrder = model.DisplayOrder;

            await _faqItemService.UpdateFaqItemAsync(faqItem);

            await UpdateFaqItemLocalesAsync(faqItem, model);

            _notificationService.SuccessNotification(
                await _localizationService.GetResourceAsync("Plugins.Misc.FaqManager.Items.Updated"));

            return continueEditing
                ? RedirectToAction("Edit", new { id = faqItem.Id })
                : RedirectToAction("List");
        }

        model = await _faqItemModelFactory.PrepareFaqItemModelAsync(model, faqItem, true);

        return View("~/Plugins/Misc.FaqManager/Admin/Views/FaqItem/Edit.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_ITEMS_MANAGE)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        var faqItem = await _faqItemService.GetFaqItemByIdAsync(id);
        if (faqItem == null)
            return RedirectToAction("List");

        try
        {
            await _faqItemService.DeleteFaqItemAsync(faqItem);

            _notificationService.SuccessNotification(
                await _localizationService.GetResourceAsync("Plugins.Misc.FaqManager.Items.Deleted"));
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

    #region Utilities

    private async Task UpdateFaqItemLocalesAsync(FaqItem faqItem, FaqItemModel model)
    {
        foreach (var locale in model.Locales)
        {
            await _localizedEntityService.SaveLocalizedValueAsync(
                faqItem,
                x => x.Question,
                locale.Question,
                locale.LanguageId);

            await _localizedEntityService.SaveLocalizedValueAsync(
                faqItem,
                x => x.Answer,
                locale.Answer,
                locale.LanguageId);
        }
    }

    #endregion
}