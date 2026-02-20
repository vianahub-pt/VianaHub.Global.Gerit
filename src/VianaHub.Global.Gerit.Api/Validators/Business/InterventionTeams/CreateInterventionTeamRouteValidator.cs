using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionTeams;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Business.InterventionTeams;

public class CreateInterventionTeamRouteValidator : AbstractValidator<CreateInterventionTeamRequest>
{
    public CreateInterventionTeamRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.InterventionId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.InterventionTeam.Create.InterventionId"));

        RuleFor(x => x.TeamId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.InterventionTeam.Create.TeamId"));
    }
}
