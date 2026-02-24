using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionTeams;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.InterventionTeams;

public class UpdateInterventionTeamRouteValidator : AbstractValidator<UpdateInterventionTeamRequest>
{
    public UpdateInterventionTeamRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.InterventionId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.InterventionTeam.Update.InterventionId"));

        RuleFor(x => x.TeamId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.InterventionTeam.Update.TeamId"));
    }
}
