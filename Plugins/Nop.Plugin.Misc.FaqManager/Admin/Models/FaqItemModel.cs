using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.FaqManager.Admin.Models;

public record FaqItemModel : BaseNopEntityModel
{
    #region Properties

    public int FaqGroupId { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Fields.Question")]
    public string Question { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Fields.Answer")]
    public string Answer { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Fields.Published")]
    public bool Published { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    public IList<FaqItemLocalizedModel> Locales { get; set; }

    #endregion
}