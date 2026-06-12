using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.FaqManager.Admin.Models;

public record FaqGroupLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Fields.Name")]
    public string Name { get; set; }
}