using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Identity.User;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Identity.User;

public class UpdateUserRouteValidator : AbstractValidator<UpdateUserRequest>
{
    private readonly ILocalizationService _localization;

    public UpdateUserRouteValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.Name)
            .MaximumLength(200).WithMessage(_localization.GetMessage("Api.Validator.User.Update.Name.MaximumLength", 200));

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage(_localization.GetMessage("Api.Validator.User.Update.PhoneNumber.MaximumLength", 20));
    }
}
