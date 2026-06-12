using Nop.Core.Configuration;

namespace Nop.Plugin.Misc.FaqManager;

public class FaqManagerSettings : ISettings
{
    /// <summary>
    /// A value indicating whether to display the FAQ count
    /// </summary>
    public bool ShowFaqCount { get; set; }
}
