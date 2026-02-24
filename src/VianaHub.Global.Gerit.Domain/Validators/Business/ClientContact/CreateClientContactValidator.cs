using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientContact;

/// <summary>
/// Validador para criação de ClientContact
/// </summary>
public class CreateClientContactValidator : AbstractValidator<ClientContactEntity>
{
    public CreateClientContactValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.ClientContact.TenantIdRequired"));

        RuleFor(x => x.ClientId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.ClientContact.ClientIdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.ClientContact.NameRequired"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Domain.ClientContact.NameMaxLength", 150));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.ClientContact.EmailRequired"))
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Domain.ClientContact.EmailMaxLength", 255))
            .EmailAddress()
            .WithMessage(localization.GetMessage("Domain.ClientContact.EmailInvalid"));

        RuleFor(x => x.Phone)
            .MaximumLength(30)
            .WithMessage(localization.GetMessage("Domain.ClientContact.PhoneMaxLength", 30))
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.ClientContact.CreatedByRequired"));
    }
}
