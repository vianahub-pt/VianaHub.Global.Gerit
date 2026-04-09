using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientIndividualFiscalData;

public class CreateClientIndividualFiscalDataValidator : AbstractValidator<ClientIndividualFiscalDataEntity>
{
    public CreateClientIndividualFiscalDataValidator()
    {
        Include(new ClientIndividualFiscalDataValidator());

        RuleFor(x => x.TenantId)
            .GreaterThan(0).WithMessage("TenantId_Required");

        RuleFor(x => x.ClientIndividualId)
            .GreaterThan(0).WithMessage("ClientIndividualId_Required");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy_Required");
    }
}
