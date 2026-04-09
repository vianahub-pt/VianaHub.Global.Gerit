using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientCompanyFiscalData;

public class CreateClientCompanyFiscalDataValidator : AbstractValidator<ClientCompanyFiscalDataEntity>
{
    public CreateClientCompanyFiscalDataValidator()
    {
        Include(new ClientCompanyFiscalDataValidator());

        RuleFor(x => x.TenantId)
            .GreaterThan(0).WithMessage("TenantId_Required");

        RuleFor(x => x.ClientCompanyId)
            .GreaterThan(0).WithMessage("ClientCompanyId_Required");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy_Required");
    }
}
