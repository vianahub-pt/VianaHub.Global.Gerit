using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Employee;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.Employee;

public class UpdateEmployeeRouteValidator : AbstractValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.Employee.Update.Name"))
            .MaximumLength(150).WithMessage(localization.GetMessage("Api.Validator.Employee.Update.Name.MaximumLength", 150));
    }
}
