using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.TeamMembersTeam;

public class DeactivateTeamMembersTeamValidator : AbstractValidator<TeamMembersTeamEntity>
{
    public DeactivateTeamMembersTeamValidator(ILocalizationService localization)
    {
        // no specific rules
    }
}
