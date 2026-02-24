using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Client;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.Client;

/// <summary>
/// Validador para UpdateClientRequest
/// </summary>
public class UpdateClientRouteValidator : AbstractValidator<UpdateClientRequest>
{
    public UpdateClientRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.Client.Update.Name"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Update.Name.MaximumLength", 150));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.Client.Update.Email"))
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Update.Email.MaximumLength", 255))
            .EmailAddress()
            .WithMessage(localization.GetMessage("Api.Validator.Client.Update.Email.Invalid"));

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.Client.Update.Phone"))
            .MaximumLength(30)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Update.Phone.MaximumLength", 30));
    }
}
