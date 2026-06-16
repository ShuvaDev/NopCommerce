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
    /// Represents system name of the "FAQ Groups" menu item in the admin area
    /// </summary>
    public static string FaqGroupMenuSystemName => "FAQ Groups";

    /// <summary>
    /// Represents system name of the "FAQ Items" menu item in the admin area
    /// </summary>
    public static string FaqItemMenuSystemName => "FAQ Items";

    /// <summary>
    /// Prefix used to clear FAQ cache
    /// </summary>
    public static string FaqPrefixCacheKey =>
        "Nop.plugin.misc.faqmanager.";

    /// <summary>
    /// Published FAQ items by group
    /// </summary>
    public static CacheKey FaqItemsByGroupIdCacheKey =>
        new("Nop.plugin.misc.faqmanager.items.bygroup.{0}");

    /// <summary>
    /// Published FAQ group by product
    /// </summary>
    public static CacheKey FaqGroupByProductIdCacheKey =>
        new("Nop.plugin.misc.faqmanager.group.byproduct.{0}");

    /// <summary>
    /// Storefront FAQ model {0} = ProductId {1} = LanguageId
    /// </summary>
    public static CacheKey ProductFaqModelCacheKey =>
        new("Nop.plugin.misc.faqmanager.productfaq.{0}-{1}");


    #region Permissions

    public static class Permissions
    {
        public const string FAQ_GROUPS_VIEW = "Faq.Groups.View";
        public const string FAQ_GROUPS_MANAGE = "Faq.Groups.Manage";
        public const string FAQ_ITEMS_VIEW = "Faq.Items.View";
        public const string FAQ_ITEMS_MANAGE = "Faq.Items.Manage";
    }

    #endregion

    #region Routes

    public static class Routes
    {
        private const string ROUTE_PREFIX = "Plugin.Misc.FaqManager.Route.";

        public static class Admin
        {
            public static string FaqGroupsRouteName => ROUTE_PREFIX + "FaqGroups";
            public static string FaqGroupCreateRouteName => ROUTE_PREFIX + "FaqGroup.Create";
            public static string FaqGroupEditRouteName => ROUTE_PREFIX + "FaqGroup.Edit";
            public static string FaqGroupDeleteRouteName => ROUTE_PREFIX + "FaqGroup.Delete";
            public static string FaqItemsRouteName => ROUTE_PREFIX + "FaqItems";
        }
    }

    #endregion
}

