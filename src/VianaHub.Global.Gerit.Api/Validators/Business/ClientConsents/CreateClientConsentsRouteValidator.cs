using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientConsents;

namespace VianaHub.Global.Gerit.Api.Validators.Business.ClientConsents;

public class CreateClientConsentsRouteValidator : AbstractValidator<CreateClientConsentsRequest>
{
    public CreateClientConsentsRouteValidator()
    {
        RuleFor(x => x.ConsentTypeId)
            .GreaterThan(0).WithMessage("ConsentTypeId_Required");

        RuleFor(x => x.GrantedDate)
            .NotEmpty().WithMessage("GrantedDate_Required");

        RuleFor(x => x.Origin)
            .NotEmpty().WithMessage("Origin_Required");
    }
}
