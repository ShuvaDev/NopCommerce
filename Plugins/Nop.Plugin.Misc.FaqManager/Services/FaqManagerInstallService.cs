using Nop.Services.Configuration;
using Nop.Services.Localization;

namespace Nop.Plugin.Misc.FaqManager.Services;

/// <summary>
/// Plugin installation service
/// </summary>
public class FaqManagerInstallService
{    
    #region Fields

    private readonly ILocalizationService _localizationService;
    private readonly ISettingService _settingService;

    #endregion

    #region Ctor

    public FaqManagerInstallService(ILocalizationService localizationService,
        ISettingService settingService)
    {
        _localizationService = localizationService;
        _settingService = settingService;
    }

    #endregion

    #region Utilities
    /// <summary>
    /// Initialize settings
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    private async Task InsertSettingsAsync()
    {
        var faqSettings = await _settingService.LoadSettingAsync<FaqManagerSettings>();

        if (!await _settingService.SettingExistsAsync(faqSettings, x => x.ShowFaqCount))
        {
            await _settingService.SaveSettingAsync(new FaqManagerSettings
            {
                ShowFaqCount = false
            });
        }
    }

    /// <summary>
    /// Add or update locales
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    private async Task InsertLocalesAsync()
    {
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
        });
    }
    #endregion

    #region Methods

    /// <summary>
    /// Adds the necessary data for the plugin to work correctly
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task InstallRequiredDataAsync()
    {
        await InsertSettingsAsync();

        await InsertLocalesAsync();
    }

    /// <summary>
    /// Removes the data inserted in <see cref="InstallRequiredDataAsync"/>
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task UninstallRequiredDataAsync()
    {
        //settings
        await _settingService.DeleteSettingAsync<FaqManagerSettings>();
    }

    #endregion
}
