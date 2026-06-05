using Nop.Plugin.Widgets.Manufacturer.Components;
using Nop.Services.Cms;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Widgets.Manufacturer;

public class ManufacturerListPlugin : BasePlugin, IWidgetPlugin
{

    #region Methods

    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string>
        {
            PublicWidgetZones.HomepageBottom
        });
    }
    public Type GetWidgetViewComponent(string widgetZone)
    {
        ArgumentNullException.ThrowIfNull(widgetZone);

        if (widgetZone.Equals(PublicWidgetZones.HomepageBottom))
            return typeof(ManufacturerListViewComponent);

        return null;
    }

    #endregion


    #region Properties
    public bool HideInWidgetList => false;
    #endregion

}
