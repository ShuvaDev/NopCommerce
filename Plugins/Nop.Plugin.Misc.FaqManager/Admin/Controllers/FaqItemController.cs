using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.FaqManager.Admin.Factories;
using Nop.Plugin.Misc.FaqManager.Admin.Models;
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
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;

    #endregion

    #region Ctor

    public FaqItemController(
        FaqItemModelFactory faqItemModelFactory,
        IFaqItemService faqItemService,
        ILocalizationService localizationService,
        INotificationService notificationService)
    {
        _faqItemModelFactory = faqItemModelFactory;
        _faqItemService = faqItemService;
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

    #region Delete

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
}