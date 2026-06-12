using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.FaqManager.Admin.Models;

public record FaqGroupModel : BaseNopEntityModel
{
    #region Properties

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Fields.Product")]
    public int ProductId { get; set; }

    public string ProductName { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Fields.Published")]
    public bool Published { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    public IList<FaqGroupLocalizedModel> Locales { get; set; }

    #endregion
}
