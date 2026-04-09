using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientIndividualFiscalData;

public class DeactivateClientIndividualFiscalDataValidator : AbstractValidator<ClientIndividualFiscalDataEntity>
{
    public DeactivateClientIndividualFiscalDataValidator()
    {
        RuleFor(x => x.IsDeleted)
            .Equal(false).WithMessage("ClientIndividualFiscalData_Deleted");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy_Required");
    }
}
