using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;

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
            ["plugins.misc.faqmanager.groups"] = "FAQ Groups",
            ["plugins.misc.faqmanager.groups.addnew"] = "Add new FAQ group",
            ["plugins.misc.faqmanager.groups.edittetails"] = "Edit FAQ group details",
            ["plugins.misc.faqmanager.groups.backtolist"] = "back to list",
            ["plugins.misc.faqmanager.groups.added"] = "The FAQ group has been added successfully.",
            ["plugins.misc.faqmanager.groups.updated"] = "The FAQ group has been updated successfully.",
            ["plugins.misc.faqmanager.groups.deleted"] = "The FAQ group has been deleted successfully.",
            ["plugins.misc.faqmanager.groups.product.alreadyassigned"] = "A FAQ group is already assigned to this product. Only one FAQ group per product is allowed.",
            ["plugins.misc.faqmanager.groups.fields.name"] = "Name",
            ["plugins.misc.faqmanager.groups.fields.name.required"] = "Please provide a name.",
            ["plugins.misc.faqmanager.groups.fields.product"] = "Product",
            ["plugins.misc.faqmanager.groups.fields.product.required"] = "Please select a product.",
            ["plugins.misc.faqmanager.groups.fields.published"] = "Published",
            ["plugins.misc.faqmanager.groups.fields.displayorder"] = "Display order",

            ["plugins.misc.faqmanager.items"] = "FAQ Items",
            ["plugins.misc.faqmanager.items.fields.question"] = "Question",
            ["plugins.misc.faqmanager.items.fields.question.required"] = "Please provide a question.",
            ["plugins.misc.faqmanager.items.fields.answer"] = "Answer",
            ["plugins.misc.faqmanager.items.fields.answer.required"] = "Please provide an answer.",
            ["plugins.misc.faqmanager.items.fields.published"] = "Published",
            ["plugins.misc.faqmanager.items.fields.displayorder"] = "Display order"
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

        //locales
        await _localizationService.DeleteLocaleResourcesAsync("Plugins.Misc.FaqManager.");
    }

    #endregion
}
