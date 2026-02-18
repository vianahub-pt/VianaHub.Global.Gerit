using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.TeamMembersTeams;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Business.TeamMembersTeams;

public class UpdateTeamMembersTeamRouteValidator : AbstractValidator<UpdateTeamMembersTeamRequest>
{
    public UpdateTeamMembersTeamRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TeamId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.TeamMembersTeam.Update.TeamId"));

        RuleFor(x => x.TeamMemberId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.TeamMembersTeam.Update.TeamMemberId"));
    }
}
