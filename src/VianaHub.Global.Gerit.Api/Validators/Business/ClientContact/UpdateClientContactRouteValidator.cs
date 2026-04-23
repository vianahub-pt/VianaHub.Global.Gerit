using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientContact;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.ClientContact;

/// <summary>
/// Validador de rota para atualização de ClientContact
/// </summary>
public class UpdateClientContactRouteValidator : AbstractValidator<UpdateClientContactRequest>
{
    public UpdateClientContactRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.ClientContact.Update.Name"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Api.Validator.ClientContact.Update.Name.MaximumLength", 150));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.ClientContact.Update.Email"))
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Api.Validator.ClientContact.Update.Email.MaximumLength", 255))
            .EmailAddress()
            .WithMessage(localization.GetMessage("Api.Validator.ClientContact.Update.Email.Invalid"));

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(30)
            .WithMessage(localization.GetMessage("Api.Validator.ClientContact.Update.Phone.MaximumLength", 30))
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
    }
}
