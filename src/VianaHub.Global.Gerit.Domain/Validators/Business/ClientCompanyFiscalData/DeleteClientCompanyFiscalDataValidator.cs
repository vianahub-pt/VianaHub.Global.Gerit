using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientCompanyFiscalData;

public class DeleteClientCompanyFiscalDataValidator : AbstractValidator<ClientCompanyFiscalDataEntity>
{
    public DeleteClientCompanyFiscalDataValidator()
    {
        RuleFor(x => x.IsDeleted)
            .Equal(false).WithMessage("ClientCompanyFiscalData_AlreadyDeleted");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy_Required");
    }
}
