using Nop.Core.Domain.Cms;
using Nop.Plugin.Misc.FaqManager.Public.Components;
using Nop.Plugin.Misc.FaqManager.Services;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Misc.FaqManager
{
    public class FaqManagerPlugin : BasePlugin, IMiscPlugin, IWidgetPlugin
    {
        #region Fields

        private readonly ISettingService _settingService;
        private readonly FaqManagerInstallService _faqManagerInstallService;
        private readonly WidgetSettings _widgetSettings;
        private readonly INopUrlHelper _nopUrlHelper;

        #endregion

        #region Ctor

        public FaqManagerPlugin(ISettingService settingService,
            FaqManagerInstallService faqManagerInstallService,
            WidgetSettings widgetSettings,
            INopUrlHelper nopUrlHelper)
        {
            _settingService = settingService;
            _faqManagerInstallService = faqManagerInstallService;
            _widgetSettings = widgetSettings;
            _nopUrlHelper = nopUrlHelper;
        }


        #endregion

        #region Methods

        public override string GetConfigurationPageUrl()
        {
            return _nopUrlHelper.RouteUrl(FaqManagerDefaults.Routes.Admin.ConfigureRouteName);
        }

        public override async Task InstallAsync()
        {
            await _faqManagerInstallService.InstallRequiredDataAsync();

            //widget
            if (!_widgetSettings.ActiveWidgetSystemNames.Contains(FaqManagerDefaults.SystemName))
            {
                _widgetSettings.ActiveWidgetSystemNames.Add(FaqManagerDefaults.SystemName);
                await _settingService.SaveSettingAsync(_widgetSettings);
            }
        }

        /// <summary>
        /// Uninstall the plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task UninstallAsync()
        {
            await _faqManagerInstallService.UninstallRequiredDataAsync();

            //widget
            if (_widgetSettings.ActiveWidgetSystemNames.Contains(FaqManagerDefaults.SystemName))
            {
                _widgetSettings.ActiveWidgetSystemNames.Remove(FaqManagerDefaults.SystemName);
                await _settingService.SaveSettingAsync(_widgetSettings);
            }
        }

        #region IWidgetPlugin

        public Type GetWidgetViewComponent(string widgetZone)
        {
            return typeof(ProductFaqViewComponent);
        }

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.ProductDetailsBottom });
        }

        #endregion

        #endregion

        #region Properties

        #region IWidgetPlugin

        public bool HideInWidgetList => true;


        #endregion

        #endregion
    }
}
