using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.Employee;

public class CreateEmployeeValidator : AbstractValidator<EmployeeEntity>
{
    public CreateEmployeeValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Employee.TenantIdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Employee.NameRequired"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Domain.Employee.NameMaxLength", 150));

        RuleFor(x => x.StatusDefinitionId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Employee.StatusDefinitionIdRequired"));

        RuleFor(x => x.StatusDomainId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Employee.StatusDomainIdRequired"));

        RuleFor(x => x.Email)
            .MaximumLength(250)
            .WithMessage(localization.GetMessage("Domain.Employee.EmailMaxLength", 250))
            .EmailAddress()
            .WithMessage(localization.GetMessage("Domain.Employee.EmailInvalid"));

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(30)
            .WithMessage(localization.GetMessage("Domain.Employee.PhoneNumberMaxLength", 30));

        RuleFor(x => x.CellPhoneNumber)
            .MaximumLength(30)
            .WithMessage(localization.GetMessage("Domain.Employee.CellPhoneNumberMaxLength", 30));

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500)
            .WithMessage(localization.GetMessage("Domain.Employee.ImageUrlMaxLength", 500));
    }
}
