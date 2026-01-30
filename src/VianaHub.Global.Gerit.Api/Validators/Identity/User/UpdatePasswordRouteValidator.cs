using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Identity.User;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Identity.User;

public class UpdatePasswordRouteValidator : AbstractValidator<UpdatePasswordRequest>
{
    private readonly ILocalizationService _localization;

    public UpdatePasswordRouteValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage(_localization.GetMessage("Api.Validator.User.UpdatePassword.CurrentPassword"));

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage(_localization.GetMessage("Api.Validator.User.UpdatePassword.NewPassword"))
            .MinimumLength(8).WithMessage(_localization.GetMessage("Api.Validator.User.UpdatePassword.NewPassword.MinimumLength", 8))
            .MaximumLength(100).WithMessage(_localization.GetMessage("Api.Validator.User.UpdatePassword.NewPassword.MaximumLength", 100))
            .Matches(@"[A-Z]").WithMessage(_localization.GetMessage("Api.Validator.User.UpdatePassword.NewPassword.RequiresUpperCase"))
            .Matches(@"[a-z]").WithMessage(_localization.GetMessage("Api.Validator.User.UpdatePassword.NewPassword.RequiresLowerCase"))
            .Matches(@"[0-9]").WithMessage(_localization.GetMessage("Api.Validator.User.UpdatePassword.NewPassword.RequiresDigit"))
            .Matches(@"[^a-zA-Z0-9]").WithMessage(_localization.GetMessage("Api.Validator.User.UpdatePassword.NewPassword.RequiresSpecialCharacter"));
    }
}
