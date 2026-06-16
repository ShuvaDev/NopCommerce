using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.FaqManager.Admin.Models;

public record FaqItemModel : BaseNopEntityModel, ILocalizedModel<FaqItemLocalizedModel>
{
    #region Ctor

    public FaqItemModel()
    {
        Locales = new List<FaqItemLocalizedModel>();
        AvailableFaqGroups = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Groups")]
    public int FaqGroupId { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Groups")]
    public string FaqGroupName { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Items.Fields.Question")]
    public string Question { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Items.Fields.Answer")]
    public string Answer { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Items.Fields.Published")]
    public bool Published { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Items.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    public IList<FaqItemLocalizedModel> Locales { get; set; }

    public IList<SelectListItem> AvailableFaqGroups { get; set; }

    #endregion
}