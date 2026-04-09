using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientCompanyFiscalData;

public class UpdateClientCompanyFiscalDataValidator : AbstractValidator<ClientCompanyFiscalDataEntity>
{
    public UpdateClientCompanyFiscalDataValidator()
    {
        Include(new ClientCompanyFiscalDataValidator());

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy_Required");
    }
}
