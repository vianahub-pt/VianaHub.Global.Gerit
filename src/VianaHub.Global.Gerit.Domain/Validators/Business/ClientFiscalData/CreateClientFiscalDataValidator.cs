using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientFiscalData;

public class CreateClientFiscalDataValidator : AbstractValidator<ClientFiscalDataEntity>
{
    public CreateClientFiscalDataValidator()
    {
        Include(new ClientFiscalDataValidator());

        RuleFor(x => x.TenantId)
            .GreaterThan(0).WithMessage("TenantId_Required");

        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("ClientId_Required");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy_Required");
    }
}
