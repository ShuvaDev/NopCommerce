using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc.Routing;
using Nop.Web.Infrastructure;

namespace Nop.Plugin.Misc.FaqManager.Infrastructure;

/// <summary>
/// Represents plugin route provider
/// </summary>
public class RouteProvider : BaseRouteProvider, IRouteProvider
{
    /// <summary>
    /// Register routes
    /// </summary>
    /// <param name="endpointRouteBuilder">Route builder</param>
    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {

        endpointRouteBuilder.MapControllerRoute(name: FaqManagerDefaults.Routes.Admin.FaqGroupsRouteName,
            pattern: "Admin/FaqGroups",
            defaults: new { controller = "FaqGroup", action = "List", area = AreaNames.ADMIN });

        endpointRouteBuilder.MapControllerRoute(name: FaqManagerDefaults.Routes.Admin.FaqGroupCreateRouteName,
            pattern: "Admin/FaqGroup/Create",
            defaults: new { controller = "FaqGroup", action = "Create", area = AreaNames.ADMIN });

        endpointRouteBuilder.MapControllerRoute(name: FaqManagerDefaults.Routes.Admin.FaqGroupEditRouteName,
            pattern: "Admin/FaqGroup/Edit/{id}",
            defaults: new { controller = "FaqGroup", action = "Edit", area = AreaNames.ADMIN });

        endpointRouteBuilder.MapControllerRoute(name: FaqManagerDefaults.Routes.Admin.FaqGroupDeleteRouteName,
            pattern: "Admin/FaqGroup/Delete/{id}",
            defaults: new { controller = "FaqGroup", action = "Delete", area = AreaNames.ADMIN });

        endpointRouteBuilder.MapControllerRoute(name: FaqManagerDefaults.Routes.Admin.FaqItemsRouteName,
            pattern: "Admin/FaqItems",
            defaults: new { controller = "FaqItem", action = "List", area = AreaNames.ADMIN });
    }

    /// <summary>
    /// Gets a priority of route provider
    /// </summary>
    public int Priority => 0;
}