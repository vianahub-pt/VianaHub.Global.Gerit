using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientIndividualFiscalData;

public class UpdateClientIndividualFiscalDataValidator : AbstractValidator<ClientIndividualFiscalDataEntity>
{
    public UpdateClientIndividualFiscalDataValidator()
    {
        Include(new ClientIndividualFiscalDataValidator());

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy_Required");
    }
}
