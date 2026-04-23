using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientCompany;

public class DeleteClientCompanyValidator : AbstractValidator<ClientCompanyEntity>
{
    public DeleteClientCompanyValidator()
    {
        RuleFor(x => x.IsDeleted)
            .Equal(false).WithMessage("ClientCompany_AlreadyDeleted");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy_Required");
    }
}
