using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.OriginType;

namespace VianaHub.Global.Gerit.Api.Validators.Business.OriginType;

public class UpdateOriginTypeRouteValidator : AbstractValidator<UpdateOriginTypeRequest>
{
    public UpdateOriginTypeRouteValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name_Required")
            .MaximumLength(100).WithMessage("Name_MaxLength");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description_Required")
            .MaximumLength(500).WithMessage("Description_MaxLength");
    }
}
