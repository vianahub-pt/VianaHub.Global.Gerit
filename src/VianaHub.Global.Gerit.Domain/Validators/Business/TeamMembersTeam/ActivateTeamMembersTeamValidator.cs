using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.TeamMembersTeam;

public class ActivateTeamMembersTeamValidator : AbstractValidator<TeamMembersTeamEntity>
{
    public ActivateTeamMembersTeamValidator(ILocalizationService localization)
    {
        // no specific rules
    }
}
