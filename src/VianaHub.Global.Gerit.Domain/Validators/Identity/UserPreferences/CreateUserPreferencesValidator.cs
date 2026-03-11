using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Identity;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Identity.UserPreferences;

public class CreateUserPreferencesValidator : AbstractValidator<UserPreferencesEntity>
{
    public CreateUserPreferencesValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.UserPreferences.TenantIdRequired"));
        RuleFor(x => x.UserId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.UserPreferences.UserIdRequired"));
        RuleFor(x => x.Appearance).NotEmpty().WithMessage(localization.GetMessage("Domain.UserPreferences.AppearanceRequired"));
        RuleFor(x => x.Locale).NotEmpty().WithMessage(localization.GetMessage("Domain.UserPreferences.LocaleRequired"));
        RuleFor(x => x.Timezone).NotEmpty().WithMessage(localization.GetMessage("Domain.UserPreferences.TimezoneRequired"));
        RuleFor(x => x.DateFormat).NotEmpty().WithMessage(localization.GetMessage("Domain.UserPreferences.DateFormatRequired"));
        RuleFor(x => x.TimeFormat).NotEmpty().WithMessage(localization.GetMessage("Domain.UserPreferences.TimeFormatRequired"));
    }
}
