using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Employee;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.Employee;

public class CreateEmployeeRouteValidator : AbstractValidator<CreateEmployeeRequest>
{
    public CreateEmployeeRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.Employee.Create.Name"))
            .MaximumLength(150).WithMessage(localization.GetMessage("Api.Validator.Employee.Create.Name.MaximumLength", 150));

        RuleFor(x => x.TaxNumber)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.Employee.Create.TaxNumber"))
            .MaximumLength(20).WithMessage(localization.GetMessage("Api.Validator.Employee.Create.TaxNumber.MaximumLength", 20));
    }
}
