using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientFiscalData;

public class ActivateClientFiscalDataValidator : AbstractValidator<ClientFiscalDataEntity>
{
    public ActivateClientFiscalDataValidator()
    {
        RuleFor(x => x.IsDeleted)
            .Equal(false).WithMessage("ClientFiscalData_Deleted");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy_Required");
    }
}
