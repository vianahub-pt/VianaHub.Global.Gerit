using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Identity;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Identity.UserPreferences;

public class UpdateUserPreferencesValidator : AbstractValidator<UserPreferencesEntity>
{
    public UpdateUserPreferencesValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage(localization.GetMessage("Domain.UserPreferences.InvalidId"));
        RuleFor(x => x.IsDeleted).Equal(false).WithMessage(localization.GetMessage("Domain.UserPreferences.CannotUpdateDeleted"));
        RuleFor(x => x.Appearance).Must(x => x == null || x == "light" || x == "dark").WithMessage(localization.GetMessage("Domain.UserPreferences.AppearanceInvalid"));
        RuleFor(x => x.TimeFormat).Must(x => x == null || x == "24h" || x == "12h").WithMessage(localization.GetMessage("Domain.UserPreferences.TimeFormatInvalid"));
    }
}
