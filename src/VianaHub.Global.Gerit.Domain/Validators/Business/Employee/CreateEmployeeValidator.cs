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

        RuleFor(x => x.TaxNumber)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Employee.TaxNumberRequired"))
            .MaximumLength(20)
            .WithMessage(localization.GetMessage("Domain.Employee.TaxNumberMaxLength", 20));
    }
}
