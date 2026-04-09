using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientCompanyFiscalData;

public class DeactivateClientCompanyFiscalDataValidator : AbstractValidator<ClientCompanyFiscalDataEntity>
{
    public DeactivateClientCompanyFiscalDataValidator()
    {
        RuleFor(x => x.IsDeleted)
            .Equal(false).WithMessage("ClientCompanyFiscalData_Deleted");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy_Required");
    }
}
