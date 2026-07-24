using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.EmployeeFiscalData;

namespace VianaHub.Global.Gerit.Api.Validators.Business.EmployeeFiscalData;

public class CreateEmployeeFiscalDataRouteValidator : AbstractValidator<CreateEmployeeFiscalDataRequest>
{
    public CreateEmployeeFiscalDataRouteValidator()
    {
        RuleFor(x => x.TaxNumber)
            .NotEmpty().WithMessage("TaxNumber_Required")
            .MaximumLength(20).WithMessage("TaxNumber_MaxLength");
    }
}
