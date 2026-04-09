using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.OriginType;

public class UpdateOriginTypeValidator : AbstractValidator<OriginTypeEntity>
{
    public UpdateOriginTypeValidator()
    {
        Include(new OriginTypeValidator());

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy_Required");
    }
}
