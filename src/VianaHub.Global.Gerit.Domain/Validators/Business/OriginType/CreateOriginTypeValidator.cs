using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.OriginType;

public class CreateOriginTypeValidator : AbstractValidator<OriginTypeEntity>
{
    public CreateOriginTypeValidator()
    {
        Include(new OriginTypeValidator());

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy_Required");
    }
}
