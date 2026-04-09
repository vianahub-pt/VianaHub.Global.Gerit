using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.Employee;

public class UpdateEmployeeValidator : AbstractValidator<EmployeeEntity>
{
    public UpdateEmployeeValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Employee.IdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Employee.NameRequired"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Domain.Employee.NameMaxLength", 150));
    }
}
