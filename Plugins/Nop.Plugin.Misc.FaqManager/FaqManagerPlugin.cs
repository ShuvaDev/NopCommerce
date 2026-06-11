using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Plugins;

namespace Nop.Plugin.Misc.FaqManager
{
    public class FaqManagerPlugin : BasePlugin, IMiscPlugin, IWidgetPlugin
    {
        #region Fields

        #endregion

        #region Ctor
        public FaqManagerPlugin()
        {
        }

        #endregion

        #region Methods

        #region IWidgetPlugin

        public Type GetWidgetViewComponent(string widgetZone)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        #region Properties

        #region IWidgetPlugin

        public bool HideInWidgetList => throw new NotImplementedException();


        #endregion

        #endregion
    }
}
