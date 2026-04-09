using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.OriginType;

public class DeleteOriginTypeValidator : AbstractValidator<OriginTypeEntity>
{
    public DeleteOriginTypeValidator()
    {
        RuleFor(x => x.IsDeleted)
            .Equal(false).WithMessage("OriginType_AlreadyDeleted");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy_Required");
    }
}
