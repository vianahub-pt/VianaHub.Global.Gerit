using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientCompanyFiscalData;

public class ActivateClientCompanyFiscalDataValidator : AbstractValidator<ClientCompanyFiscalDataEntity>
{
    public ActivateClientCompanyFiscalDataValidator()
    {
        RuleFor(x => x.IsDeleted)
            .Equal(false).WithMessage("ClientCompanyFiscalData_Deleted");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy_Required");
    }
}
