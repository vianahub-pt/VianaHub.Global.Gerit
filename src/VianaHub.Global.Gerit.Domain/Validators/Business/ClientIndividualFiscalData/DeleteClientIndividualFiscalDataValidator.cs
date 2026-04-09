using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientIndividualFiscalData;

public class DeleteClientIndividualFiscalDataValidator : AbstractValidator<ClientIndividualFiscalDataEntity>
{
    public DeleteClientIndividualFiscalDataValidator()
    {
        RuleFor(x => x.IsDeleted)
            .Equal(false).WithMessage("ClientIndividualFiscalData_AlreadyDeleted");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy_Required");
    }
}
