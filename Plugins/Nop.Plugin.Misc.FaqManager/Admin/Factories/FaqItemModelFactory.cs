using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.FaqManager.Admin.Models;
using Nop.Plugin.Misc.FaqManager.Domain;
using Nop.Plugin.Misc.FaqManager.Services;
using Nop.Services.Localization;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.FaqManager.Admin.Factories;

public class FaqItemModelFactory
{
    #region Fields

    private readonly IFaqItemService _faqItemService;
    private readonly IFaqGroupService _faqGroupService;
    private readonly ILocalizedEntityService _localizedEntityService;
    private readonly ILanguageService _languageService;

    #endregion

    #region Ctor

    public FaqItemModelFactory(
        IFaqItemService faqItemService,
        IFaqGroupService faqGroupService,
        ILocalizedEntityService localizedEntityService,
        ILanguageService languageService)
    {
        _faqItemService = faqItemService;
        _faqGroupService = faqGroupService;
        _localizedEntityService = localizedEntityService;
        _languageService = languageService;
    }

    #endregion

    #region Methods

    public virtual async Task<FaqItemSearchModel> PrepareFaqItemSearchModelAsync(
        FaqItemSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        searchModel.SetGridPageSize();

        searchModel.AvailablePublishedOptions.Add(new SelectListItem { Text = "All", Value = "" });
        searchModel.AvailablePublishedOptions.Add(new SelectListItem { Text = "Published only", Value = "true" });
        searchModel.AvailablePublishedOptions.Add(new SelectListItem { Text = "Unpublished only", Value = "false" });

        var faqGroups = await _faqGroupService.GetAllFaqGroupsAsync();
        searchModel.AvailableFaqGroups.Add(new SelectListItem { Text = "All", Value = "0" });
        foreach (var faqGroup in faqGroups)
        {
            searchModel.AvailableFaqGroups.Add(new SelectListItem
            {
                Text = faqGroup.Name,
                Value = faqGroup.Id.ToString()
            });
        }

        return searchModel;
    }

    public virtual async Task<FaqItemListModel> PrepareFaqItemListModelAsync(
        FaqItemSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        var faqItems = await _faqItemService.GetAllFaqItemsAsync(
            question: searchModel.SearchQuestion,
            faqGroupId: searchModel.SearchFaqGroupId,
            published: searchModel.SearchPublished,
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize);

        var model = await new FaqItemListModel().PrepareToGridAsync(
            searchModel,
            faqItems,
            () => faqItems.SelectAwait(async faqItem =>
            {
                var faqGroup = await _faqGroupService.GetFaqGroupByIdAsync(faqItem.FaqGroupId);

                return new FaqItemModel
                {
                    Id = faqItem.Id,
                    FaqGroupId = faqItem.FaqGroupId,
                    FaqGroupName = faqGroup?.Name,
                    Question = faqItem.Question,
                    Answer = faqItem.Answer,
                    Published = faqItem.Published,
                    DisplayOrder = faqItem.DisplayOrder
                };
            }));

        return model;
    }

    public virtual async Task<FaqItemModel> PrepareFaqItemModelAsync(
        FaqItemModel model,
        FaqItem faqItem,
        bool excludeProperties = false)
    {
        if (faqItem != null)
        {
            model ??= new FaqItemModel();

            model.Id = faqItem.Id;
            model.FaqGroupId = faqItem.FaqGroupId;
            model.Question = faqItem.Question;
            model.Answer = faqItem.Answer;
            model.Published = faqItem.Published;
            model.DisplayOrder = faqItem.DisplayOrder;
        }

        if (!excludeProperties && model != null)
        {
            var languages = await _languageService.GetAllLanguagesAsync();

            foreach (var language in languages)
            {
                var locale = new FaqItemLocalizedModel
                {
                    LanguageId = language.Id
                };

                if (faqItem != null)
                {
                    locale.Question = await _localizedEntityService.GetLocalizedValueAsync(
                        language.Id,
                        faqItem.Id,
                        nameof(FaqItem),
                        nameof(FaqItem.Question));

                    locale.Answer = await _localizedEntityService.GetLocalizedValueAsync(
                        language.Id,
                        faqItem.Id,
                        nameof(FaqItem),
                        nameof(FaqItem.Answer));
                }

                model.Locales.Add(locale);
            }
        }

        if (model != null)
        {
            var faqGroups = await _faqGroupService.GetAllFaqGroupsAsync();
            model.AvailableFaqGroups.Add(new SelectListItem { Text = "Select a FAQ group", Value = "0" });
            foreach (var faqGroup in faqGroups)
            {
                model.AvailableFaqGroups.Add(new SelectListItem
                {
                    Text = faqGroup.Name,
                    Value = faqGroup.Id.ToString(),
                    Selected = faqGroup.Id == model.FaqGroupId
                });
            }
        }

        return model;
    }

    #endregion
}