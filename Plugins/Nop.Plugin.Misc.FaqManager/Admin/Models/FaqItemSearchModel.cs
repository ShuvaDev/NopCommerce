using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.FaqManager.Admin.Models;

public record FaqItemSearchModel : BaseSearchModel
{
    #region Ctor

    public FaqItemSearchModel()
    {
        AvailablePublishedOptions = new List<SelectListItem>();
        AvailableFaqGroups = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Items.Fields.Question")]
    public string SearchQuestion { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Items.Fields.Published")]
    public bool? SearchPublished { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Groups")]
    public int SearchFaqGroupId { get; set; }

    public IList<SelectListItem> AvailablePublishedOptions { get; set; }

    public IList<SelectListItem> AvailableFaqGroups { get; set; }

    #endregion
}
