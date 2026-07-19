using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientContact;

/// <summary>
/// Validador para cria��o de ClientContact
/// </summary>
public class CreateClientContactValidator : AbstractValidator<ClientContactPersonsEntity>
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

        RuleFor(x => x.JobTitle)
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.ClientContact.JobTitleMaxLength", 100))
            .When(x => !string.IsNullOrWhiteSpace(x.JobTitle));

        RuleFor(x => x.Department)
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.ClientContact.DepartmentMaxLength", 100))
            .When(x => !string.IsNullOrWhiteSpace(x.Department));

        RuleFor(x => x.CellPhoneNumber)
            .MaximumLength(30)
            .WithMessage(localization.GetMessage("Domain.ClientContact.CellPhoneNumberMaxLength", 30))
            .When(x => !string.IsNullOrWhiteSpace(x.CellPhoneNumber));

        // Quando IsCellPhoneWhatsapp for true, CellPhoneNumber é obrigatório
        RuleFor(x => x.CellPhoneNumber)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.ClientContact.CellPhoneNumberRequiredForWhatsapp"))
            .When(x => x.IsCellPhoneWhatsapp);

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.ClientContact.CreatedByRequired"));
    }
}
