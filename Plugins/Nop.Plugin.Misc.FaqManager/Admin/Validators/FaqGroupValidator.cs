using FluentValidation;
using Nop.Plugin.Misc.FaqManager.Admin.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Misc.FaqManager.Admin.Validators;

/// <summary>
/// Validator for FAQ group model
/// </summary>
public class FaqGroupValidator : BaseNopValidator<FaqGroupModel>
{
    #region Ctor

    public FaqGroupValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync(
                "Plugins.Misc.FaqManager.Groups.Fields.Name.Required"));

        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessageAwait(localizationService.GetResourceAsync(
                "Plugins.Misc.FaqManager.Groups.Fields.Product.Required"));
    }

    #endregion
}
