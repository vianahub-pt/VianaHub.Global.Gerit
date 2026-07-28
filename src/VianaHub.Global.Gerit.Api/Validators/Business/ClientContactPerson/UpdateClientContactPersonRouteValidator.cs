using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientContact;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.ClientContact;

/// <summary>
/// Validador de rota para atualização de ClientContact
/// </summary>
public class UpdateClientContactPersonRouteValidator : AbstractValidator<UpdateClientContactPersonRequest>
{
    public UpdateClientContactPersonRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.ClientContactPerson.Update.Name"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Api.Validator.ClientContactPerson.Update.Name.MaximumLength", 150));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.ClientContactPerson.Update.Email"))
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Api.Validator.ClientContactPerson.Update.Email.MaximumLength", 255))
            .EmailAddress()
            .WithMessage(localization.GetMessage("Api.Validator.ClientContactPerson.Update.Email.Invalid"));

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(30)
            .WithMessage(localization.GetMessage("Api.Validator.ClientContactPerson.Update.Phone.MaximumLength", 30))
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
    }
}
