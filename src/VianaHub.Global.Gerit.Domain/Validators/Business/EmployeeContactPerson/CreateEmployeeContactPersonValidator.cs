using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.EmployeeContact;

/// <summary>
/// Validator para cria��o de EmployeeContact
/// </summary>
public class CreateEmployeeContactPersonValidator : AbstractValidator<EmployeeContactPersonsEntity>
{
    private readonly ILocalizationService _localization;

    public CreateEmployeeContactPersonValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.EmployeeContactPerson.TenantIdRequired"));

        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.EmployeeContactPerson.EmployeeIdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Domain.EmployeeContactPerson.NameRequired"))
            .MaximumLength(150)
            .WithMessage(_localization.GetMessage("Domain.EmployeeContactPerson.NameMaxLength"));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Domain.EmployeeContactPerson.EmailRequired"))
            .MaximumLength(255)
            .WithMessage(_localization.GetMessage("Domain.EmployeeContactPerson.EmailMaxLength"))
            .EmailAddress()
            .WithMessage(_localization.GetMessage("Domain.EmployeeContactPerson.EmailInvalid"));

        RuleFor(x => x.Phone)
            .MaximumLength(30)
            .When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage(_localization.GetMessage("Domain.EmployeeContactPerson.PhoneMaxLength"));

        RuleFor(x => x.JobTitle)
            .MaximumLength(100)
            .WithMessage(_localization.GetMessage("Domain.EmployeeContactPerson.JobTitleMaxLength", 100))
            .When(x => !string.IsNullOrWhiteSpace(x.JobTitle));

        RuleFor(x => x.Department)
            .MaximumLength(100)
            .WithMessage(_localization.GetMessage("Domain.EmployeeContactPerson.DepartmentMaxLength", 100))
            .When(x => !string.IsNullOrWhiteSpace(x.Department));

        RuleFor(x => x.CellPhoneNumber)
            .MaximumLength(30)
            .WithMessage(_localization.GetMessage("Domain.EmployeeContactPerson.CellPhoneNumberMaxLength", 30))
            .When(x => !string.IsNullOrWhiteSpace(x.CellPhoneNumber));

        // Quando IsCellPhoneWhatsapp for true, CellPhoneNumber é obrigatório
        RuleFor(x => x.CellPhoneNumber)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Domain.EmployeeContactPerson.CellPhoneNumberRequiredForWhatsapp"))
            .When(x => x.IsCellPhoneWhatsapp);

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.EmployeeContactPerson.CreatedByRequired"));
    }
}
