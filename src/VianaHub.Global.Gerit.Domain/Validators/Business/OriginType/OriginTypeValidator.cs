using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.OriginType;

public class OriginTypeValidator : AbstractValidator<OriginTypeEntity>
{
    public OriginTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name_Required")
            .MaximumLength(100).WithMessage("Name_MaxLength");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description_Required")
            .MaximumLength(500).WithMessage("Description_MaxLength");
    }
}
