using Nop.Core.Caching;

namespace Nop.Plugin.Misc.FaqManager;

/// <summary>
/// Represents plugin constants
/// </summary>
public class FaqManagerDefaults
{
    /// <summary>
    /// Gets a plugin system name
    /// </summary>
    public static string SystemName => "Misc.FaqManager";

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


    #region Permissions

    public static class Permissions
    {
        public const string FAQ_GROUPS_VIEW = "Faq.Groups.View";
        public const string FAQ_GROUPS_MANAGE = "Faq.Groups.Manage";
        public const string FAQ_ITEMS_VIEW = "Faq.Items.View";
        public const string FAQ_ITEMS_MANAGE = "Faq.Items.Manage";
    }

    #endregion
}

