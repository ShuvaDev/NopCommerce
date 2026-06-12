using Nop.Core.Domain.Customers;
using Nop.Services.Security;

namespace Nop.Plugin.Misc.FaqManager.Services;

internal class FaqManagerPermissionConfigManager : IPermissionConfigManager
{
    /// <summary>
    /// Gets all permission configurations
    /// </summary>
    public IList<PermissionConfig> AllConfigs => new List<PermissionConfig>
    {
        new ("Admin area. Faq Groups. View", FaqManagerDefaults.Permissions.FAQ_GROUPS_VIEW, nameof(StandardPermission.ContentManagement), NopCustomerDefaults.AdministratorsRoleName),
        new ("Admin area. Faq Group. Create, edit, delete", FaqManagerDefaults.Permissions.FAQ_GROUPS_MANAGE, nameof(StandardPermission.ContentManagement), NopCustomerDefaults.AdministratorsRoleName),
        new ("Admin area. Faq Items. View", FaqManagerDefaults.Permissions.FAQ_ITEMS_VIEW, nameof(StandardPermission.ContentManagement), NopCustomerDefaults.AdministratorsRoleName),
        new ("Admin area. Faq Item. Create, edit, delete", FaqManagerDefaults.Permissions.FAQ_ITEMS_MANAGE, nameof(StandardPermission.ContentManagement), NopCustomerDefaults.AdministratorsRoleName),
    };
}