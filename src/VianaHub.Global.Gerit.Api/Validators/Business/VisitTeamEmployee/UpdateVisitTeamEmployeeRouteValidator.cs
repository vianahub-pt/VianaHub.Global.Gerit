using FluentValidation;

namespace VianaHub.Global.Gerit.Api.Validators.Business.VisitTeamEmployee;

public class UpdateVisitTeamEmployeeRouteValidator : AbstractValidator<int>
{
    public UpdateVisitTeamEmployeeRouteValidator()
    {
        RuleFor(x => x)
            .GreaterThan(0)
            .WithMessage("Domain.VisitTeamEmployee.IdInvalid");
    }
}
