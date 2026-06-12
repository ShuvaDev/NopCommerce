using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.FaqManager.Admin.Models;

public record FaqItemLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Fields.Question")]
    public string Question { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Fields.Answer")]
    public string Answer { get; set; }
}
