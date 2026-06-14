using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Misc.FaqManager.Domain;
using Nop.Plugin.Misc.FaqManager.Public.Models;
using Nop.Plugin.Misc.FaqManager.Public.Models.JsonLd;
using Nop.Plugin.Misc.FaqManager.Services;
using Nop.Services.Configuration;
using Nop.Services.Html;
using Nop.Services.Localization;

namespace Nop.Plugin.Misc.FaqManager.Public.Factories;

public class ProductFaqModelFactory
{
    #region Fields

    private readonly IFaqGroupService _faqGroupService;
    private readonly IFaqItemService _faqItemService;
    private readonly IStaticCacheManager _staticCacheManager;
    private readonly IWorkContext _workContext;
    private readonly ILocalizedEntityService _localizedEntityService;
    private readonly IHtmlFormatter _htmlFormatter;
    private readonly ISettingService _settingService;
    private readonly IStoreContext _storeContext;

    #endregion

    #region Ctor

    public ProductFaqModelFactory(
        IFaqGroupService faqGroupService,
        IFaqItemService faqItemService,
        IStaticCacheManager staticCacheManager,
        IWorkContext workContext,
        ILocalizedEntityService localizedEntityService,
        IHtmlFormatter htmlFormatter,
        ISettingService settingService,
        IStoreContext storeContext)
    {
        _faqGroupService = faqGroupService;
        _faqItemService = faqItemService;
        _staticCacheManager = staticCacheManager;
        _workContext = workContext;
        _localizedEntityService = localizedEntityService;
        _htmlFormatter = htmlFormatter;
        _settingService = settingService;
        _storeContext = storeContext;
    }

    #endregion

    #region Methods

    public virtual async Task<ProductFaqModel?> PrepareProductFaqModelAsync(int productId)
    {
        var language = await _workContext.GetWorkingLanguageAsync();
        var storeId = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var faqManagerSettings = await _settingService.LoadSettingAsync<FaqManagerSettings>(storeId);

        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
            FaqManagerDefaults.ProductFaqModelCacheKey,
            productId,
            language.Id);

        return await _staticCacheManager.GetAsync(cacheKey, async () =>
        {
            var faqGroup =
                await _faqGroupService.GetPublishedFaqGroupByProductIdAsync(productId);

            if (faqGroup == null)
                return null;

            var faqItems =
                await _faqItemService.GetPublishedFaqItemsByGroupIdAsync(faqGroup.Id);

            if (!faqItems.Any())
                return null;

            var model = new ProductFaqModel();

            foreach (var faqItem in faqItems.OrderBy(x => x.DisplayOrder))
            {
                var question =
                    await _localizedEntityService.GetLocalizedValueAsync(
                        language.Id,
                        faqItem.Id,
                        nameof(FaqItem),
                        nameof(FaqItem.Question));

                var answer =
                    await _localizedEntityService.GetLocalizedValueAsync(
                        language.Id,
                        faqItem.Id,
                        nameof(FaqItem),
                        nameof(FaqItem.Answer));

                model.Items.Add(new FaqItemModel
                {
                    Question = string.IsNullOrEmpty(question)
                        ? faqItem.Question
                        : question,

                    Answer = string.IsNullOrEmpty(answer)
                        ? faqItem.Answer
                        : answer
                });
            }

            model.JsonLd = BuildJsonLd(model.Items);
            model.ShowFaqCount = faqManagerSettings.ShowFaqCount;
            model.FaqCount = model.Items.Count;

            return model;
        });
    }

    #endregion

    #region Utilities

    protected virtual string BuildJsonLd(IList<FaqItemModel> items)
    {
        var model = new JsonLdFaqPageModel();

        foreach (var item in items)
        {
            model.MainEntity.Add(new JsonLdQuestionModel
            {
                Name = item.Question,
                AcceptedAnswer = new JsonLdAnswerModel
                {
                    Text = _htmlFormatter
                        .ConvertHtmlToPlainText(
                            item.Answer,
                            decode: true,
                            replaceAnchorTags: true)
                        .Trim()
                }
            });
        }

        return JsonConvert.SerializeObject(model);
    }

    #endregion
}