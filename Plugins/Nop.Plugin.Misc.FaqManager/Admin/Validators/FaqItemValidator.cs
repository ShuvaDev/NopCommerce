using FluentValidation;
using Nop.Plugin.Misc.FaqManager.Admin.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Misc.FaqManager.Admin.Validators;

/// <summary>
/// Validator for FAQ item model
/// </summary>
public class FaqItemValidator : BaseNopValidator<FaqItemModel>
{
    #region Ctor

    public FaqItemValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.FaqGroupId)
            .GreaterThan(0)
            .WithMessageAwait(localizationService.GetResourceAsync(
                "Plugins.Misc.FaqManager.Items.Fields.FaqGroup.Required"));

        RuleFor(x => x.Question)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync(
                "Plugins.Misc.FaqManager.Items.Fields.Question.Required"));

        RuleFor(x => x.Answer)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync(
                "Plugins.Misc.FaqManager.Items.Fields.Answer.Required"));
    }

    #endregion
}
