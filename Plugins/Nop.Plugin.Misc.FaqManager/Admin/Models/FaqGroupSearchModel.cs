using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.FaqManager.Admin.Models;

public record FaqGroupSearchModel : BaseSearchModel
{
    #region Properties

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Groups.Fields.Name")]
    public string SearchName { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Groups.Fields.Product")]
    public int SearchProductId { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Groups.Fields.Published")]
    public bool? SearchPublished { get; set; }

    #endregion
}
