using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.TeamMembersTeam;

public class DeleteTeamMembersTeamValidator : AbstractValidator<TeamMembersTeamEntity>
{
    public DeleteTeamMembersTeamValidator(ILocalizationService localization)
    {
        // no specific rules
    }
}
