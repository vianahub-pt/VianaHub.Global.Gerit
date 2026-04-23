using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientFiscalData;

public class UpdateClientFiscalDataValidator : AbstractValidator<ClientFiscalDataEntity>
{
    public UpdateClientFiscalDataValidator()
    {
        Include(new ClientFiscalDataValidator());

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy_Required");
    }
}
