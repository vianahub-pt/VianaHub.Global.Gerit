using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitTeams;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.VisitTeams;

public class CreateVisitTeamRouteValidator : AbstractValidator<CreateVisitTeamRequest>
{
    public CreateVisitTeamRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.VisitId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.VisitTeam.Create.VisitTeamId"));

        RuleFor(x => x.TeamId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.VisitTeam.Create.TeamId"));
    }
}
