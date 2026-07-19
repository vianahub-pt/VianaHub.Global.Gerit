using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Billing.TenantContact;

/// <summary>
/// Validador para criação de TenantContact
/// </summary>
public class CreateTenantContactValidator : AbstractValidator<TenantContactPersonsEntity>
{
    public CreateTenantContactValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.TenantContact.TenantIdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.TenantContact.NameRequired"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Domain.TenantContact.NameMaxLength", 200));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.TenantContact.EmailRequired"))
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Domain.TenantContact.EmailMaxLength", 255))
            .EmailAddress()
            .WithMessage(localization.GetMessage("Domain.TenantContact.EmailInvalid"));

        RuleFor(x => x.Phone)
            .MaximumLength(50)
            .WithMessage(localization.GetMessage("Domain.TenantContact.PhoneMaxLength", 50))
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x.JobTitle)
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.TenantContact.JobTitleMaxLength", 100))
            .When(x => !string.IsNullOrWhiteSpace(x.JobTitle));

        RuleFor(x => x.Department)
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.TenantContact.DepartmentMaxLength", 100))
            .When(x => !string.IsNullOrWhiteSpace(x.Department));

        RuleFor(x => x.CellPhoneNumber)
            .MaximumLength(30)
            .WithMessage(localization.GetMessage("Domain.TenantContact.CellPhoneNumberMaxLength", 30))
            .When(x => !string.IsNullOrWhiteSpace(x.CellPhoneNumber));

        // Quando IsCellPhoneWhatsapp for true, CellPhoneNumber é obrigatório
        RuleFor(x => x.CellPhoneNumber)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.TenantContact.CellPhoneNumberRequiredForWhatsapp"))
            .When(x => x.IsCellPhoneWhatsapp);

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.TenantContact.CreatedByRequired"));
    }
}
