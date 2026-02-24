using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientContact;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.ClientContact;

/// <summary>
/// Validador de rota para criação de ClientContact
/// </summary>
public class CreateClientContactRouteValidator : AbstractValidator<CreateClientContactRequest>
{
    public CreateClientContactRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.ClientId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Api.Validator.ClientContact.Create.ClientId"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.ClientContact.Create.Name"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Api.Validator.ClientContact.Create.Name.MaximumLength", 150));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.ClientContact.Create.Email"))
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Api.Validator.ClientContact.Create.Email.MaximumLength", 255))
            .EmailAddress()
            .WithMessage(localization.GetMessage("Api.Validator.ClientContact.Create.Email.Invalid"));

        RuleFor(x => x.Phone)
            .MaximumLength(30)
            .WithMessage(localization.GetMessage("Api.Validator.ClientContact.Create.Phone.MaximumLength", 30))
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));
    }
}
