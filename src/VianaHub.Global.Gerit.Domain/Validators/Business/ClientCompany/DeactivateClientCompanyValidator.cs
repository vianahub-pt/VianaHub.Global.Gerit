using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientCompany;

public class DeactivateClientCompanyValidator : AbstractValidator<ClientCompanyEntity>
{
    public DeactivateClientCompanyValidator()
    {
        RuleFor(x => x.IsDeleted)
            .Equal(false).WithMessage("ClientCompany_Deleted");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy_Required");
    }
}
