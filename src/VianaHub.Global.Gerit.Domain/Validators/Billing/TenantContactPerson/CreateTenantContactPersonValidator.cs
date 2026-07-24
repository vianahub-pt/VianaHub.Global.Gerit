using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Billing.TenantContactPerson;

/// <summary>
/// Validador para criação de TenantContact
/// </summary>
public class CreateTenantContactPersonValidator : AbstractValidator<TenantContactPersonsEntity>
{
    public CreateTenantContactPersonValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.TenantContactPerson.TenantIdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.TenantContactPerson.NameRequired"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Domain.TenantContactPerson.NameMaxLength", 200));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.TenantContactPerson.EmailRequired"))
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Domain.TenantContactPerson.EmailMaxLength", 255))
            .EmailAddress()
            .WithMessage(localization.GetMessage("Domain.TenantContactPerson.EmailInvalid"));

        RuleFor(x => x.Phone)
            .MaximumLength(50)
            .WithMessage(localization.GetMessage("Domain.TenantContactPerson.PhoneMaxLength", 50))
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x.JobTitle)
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.TenantContactPerson.JobTitleMaxLength", 100))
            .When(x => !string.IsNullOrWhiteSpace(x.JobTitle));

        RuleFor(x => x.Department)
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.TenantContactPerson.DepartmentMaxLength", 100))
            .When(x => !string.IsNullOrWhiteSpace(x.Department));

        RuleFor(x => x.CellPhoneNumber)
            .MaximumLength(30)
            .WithMessage(localization.GetMessage("Domain.TenantContactPerson.CellPhoneNumberMaxLength", 30))
            .When(x => !string.IsNullOrWhiteSpace(x.CellPhoneNumber));

        // Quando IsCellPhoneWhatsapp for true, CellPhoneNumber é obrigatório
        RuleFor(x => x.CellPhoneNumber)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.TenantContactPerson.CellPhoneNumberRequiredForWhatsapp"))
            .When(x => x.IsCellPhoneWhatsapp);

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.TenantContactPerson.CreatedByRequired"));
    }
}
