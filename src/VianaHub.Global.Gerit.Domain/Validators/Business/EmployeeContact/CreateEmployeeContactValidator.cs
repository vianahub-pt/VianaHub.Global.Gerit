using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.EmployeeContact;

/// <summary>
/// Validator para cria��o de EmployeeContact
/// </summary>
public class CreateEmployeeContactValidator : AbstractValidator<EmployeeContactEntity>
{
    private readonly ILocalizationService _localization;

    public CreateEmployeeContactValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.EmployeeContact.TenantIdRequired"));

        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.EmployeeContact.EmployeeIdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Domain.EmployeeContact.NameRequired"))
            .MaximumLength(150)
            .WithMessage(_localization.GetMessage("Domain.EmployeeContact.NameMaxLength"));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Domain.EmployeeContact.EmailRequired"))
            .MaximumLength(255)
            .WithMessage(_localization.GetMessage("Domain.EmployeeContact.EmailMaxLength"))
            .EmailAddress()
            .WithMessage(_localization.GetMessage("Domain.EmployeeContact.EmailInvalid"));

        RuleFor(x => x.Phone)
            .MaximumLength(30)
            .When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage(_localization.GetMessage("Domain.EmployeeContact.PhoneMaxLength"));

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.EmployeeContact.CreatedByRequired"));
    }
}
