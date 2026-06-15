using Nop.Services.Cms;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Misc.FaqManager.Services;

/// <summary>
/// Represents the plugin event consumer
/// </summary>
public class AdminMenuCreatedEventConsumer : IConsumer<AdminMenuCreatedEvent>
{
    #region Fields

    private readonly ILocalizationService _localizationService;
    private readonly INopUrlHelper _nopUrlHelper;
    private readonly IWidgetPluginManager _pluginManager;

    #endregion

    #region Ctor

    public AdminMenuCreatedEventConsumer(ILocalizationService localizationService,
        INopUrlHelper nopUrlHelper,
        IWidgetPluginManager pluginManager)
    {
        _localizationService = localizationService;
        _nopUrlHelper = nopUrlHelper;
        _pluginManager = pluginManager;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Handle admin menu created event
    /// </summary>
    /// <param name="eventMessage">Event message</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(AdminMenuCreatedEvent eventMessage)
    {
        var plugin = await _pluginManager.LoadPluginBySystemNameAsync(FaqManagerDefaults.SystemName);

        //the LoadPluginBySystemNameAsync method returns only plugins that are already fully installed,
        //while the IConsumer<AdminMenuCreatedEvent> event can be called before the installation is complete
        if (plugin == null || !_pluginManager.IsPluginActive(plugin))
            return;

        eventMessage.RootMenuItem.InsertAfter("Message templates", new()
        {
            SystemName = FaqManagerDefaults.FaqGroupMenuSystemName,
            Title = await _localizationService.GetResourceAsync("plugins.misc.faqmanager.groups"),
            IconClass = "far fa-dot-circle",
            Url = _nopUrlHelper.RouteUrl(FaqManagerDefaults.Routes.Admin.FaqGroupsRouteName),
            PermissionNames = new List<string> { FaqManagerDefaults.Permissions.FAQ_GROUPS_VIEW }
        });

        eventMessage.RootMenuItem.InsertAfter(FaqManagerDefaults.FaqGroupMenuSystemName, new()
        {
            SystemName = FaqManagerDefaults.FaqItemMenuSystemName,
            Title = await _localizationService.GetResourceAsync("plugins.misc.faqmanager.items"),
            IconClass = "far fa-dot-circle",
            Url = _nopUrlHelper.RouteUrl(FaqManagerDefaults.Routes.Admin.FaqItemsRouteName),
            PermissionNames = new List<string> { FaqManagerDefaults.Permissions.FAQ_ITEMS_VIEW }
        });
    }

    #endregion
}