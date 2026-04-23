using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.OriginType;

public class ActivateOriginTypeValidator : AbstractValidator<OriginTypeEntity>
{
    public ActivateOriginTypeValidator()
    {
        RuleFor(x => x.IsDeleted)
            .Equal(false).WithMessage("OriginType_Deleted");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy_Required");
    }
}
