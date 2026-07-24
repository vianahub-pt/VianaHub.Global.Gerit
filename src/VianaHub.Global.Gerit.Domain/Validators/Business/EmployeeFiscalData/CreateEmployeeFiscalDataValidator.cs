using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.EmployeeFiscalData;

public class CreateEmployeeFiscalDataValidator : AbstractValidator<EmployeeFiscalDataEntity>
{
    public CreateEmployeeFiscalDataValidator()
    {
        Include(new EmployeeFiscalDataValidator());

        RuleFor(x => x.TenantId)
            .GreaterThan(0).WithMessage("TenantId_Required");

        RuleFor(x => x.EmployeeId)
            .GreaterThan(0).WithMessage("EmployeeId_Required");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy_Required");
    }
}
