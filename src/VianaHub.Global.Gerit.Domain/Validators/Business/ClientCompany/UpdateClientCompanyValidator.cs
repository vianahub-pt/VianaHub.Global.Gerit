using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientCompany;

public class UpdateClientCompanyValidator : AbstractValidator<ClientCompanyEntity>
{
    public UpdateClientCompanyValidator()
    {
        Include(new ClientCompanyValidator());

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy_Required");
    }
}
