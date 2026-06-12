using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.FaqManager.Admin.Models;

public record FaqGroupSearchModel : BaseSearchModel
{
    #region Properties

    public string SearchName { get; set; }

    public int SearchProductId { get; set; }

    public bool? SearchPublished { get; set; }

    #endregion
}
