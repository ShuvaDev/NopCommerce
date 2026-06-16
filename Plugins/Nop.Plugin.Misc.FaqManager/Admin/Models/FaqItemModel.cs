using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.FaqManager.Admin.Models;

public record FaqItemModel : BaseNopEntityModel, ILocalizedModel<FaqItemLocalizedModel>
{
    #region Ctor

    public FaqItemModel()
    {
        Locales = new List<FaqItemLocalizedModel>();
    }

    #endregion

    #region Properties

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

    #endregion
}