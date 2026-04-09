using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientCompany;

public class CreateClientCompanyValidator : AbstractValidator<ClientCompanyEntity>
{
    public CreateClientCompanyValidator()
    {
        Include(new ClientCompanyValidator());

        RuleFor(x => x.TenantId)
            .GreaterThan(0).WithMessage("TenantId_Required");

        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("ClientId_Required");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy_Required");
    }
}
