using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.FaqManager.Admin.Factories;
using Nop.Plugin.Misc.FaqManager.Admin.Models;
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

    #endregion

    #region Ctor

    public FaqGroupController(
        FaqGroupModelFactory faqGroupModelFactory)
    {
        _faqGroupModelFactory = faqGroupModelFactory;
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
}