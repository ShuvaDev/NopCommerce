using Nop.Core.Caching;

namespace Nop.Plugin.Misc.FaqManager;

/// <summary>
/// Represents plugin constants
/// </summary>
public class FaqManagerDefaults
{
    /// <summary>
    /// Prefix used to clear FAQ cache
    /// </summary>
    public static string FaqPrefixCacheKey =>
        "Nop.plugin.misc.faqmanager.";

    /// <summary>
    /// Cache key for published FAQ items by group identifier
    /// </summary>
    public static CacheKey FaqItemsByGroupIdCacheKey =>
        new("Nop.plugin.misc.faqmanager.items.bygroup.{0}");
}

