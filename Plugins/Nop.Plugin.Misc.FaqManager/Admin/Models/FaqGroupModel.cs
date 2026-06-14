using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.FaqManager.Admin.Models;

public record FaqGroupModel : BaseNopEntityModel, ILocalizedModel<FaqGroupLocalizedModel>
{
    #region Ctor

    public FaqGroupModel()
    {
        Locales = new List<FaqGroupLocalizedModel>();
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Groups.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Groups.Fields.Product")]
    public int ProductId { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Groups.Fields.Product")]
    public string ProductName { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Groups.Fields.Published")]
    public bool Published { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Groups.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    public IList<FaqGroupLocalizedModel> Locales { get; set; }

    public FaqItemSearchModel FaqItemSearchModel { get; set; }

    #endregion
}
