using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.FaqManager.Admin.Models;

public record ConfigurationModel : BaseNopModel
{
    [NopResourceDisplayName("Plugins.Misc.FaqManager.Settings.ShowFaqCount")]
    public bool ShowFaqCount { get; set; }
}