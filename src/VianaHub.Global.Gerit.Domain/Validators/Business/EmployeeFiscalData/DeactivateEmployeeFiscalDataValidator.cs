using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.EmployeeFiscalData;

public class DeactivateEmployeeFiscalDataValidator : AbstractValidator<EmployeeFiscalDataEntity>
{
    public DeactivateEmployeeFiscalDataValidator()
    {
        RuleFor(x => x.IsDeleted)
            .Equal(false).WithMessage("EmployeeFiscalData_Deleted");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy_Required");
    }
}
