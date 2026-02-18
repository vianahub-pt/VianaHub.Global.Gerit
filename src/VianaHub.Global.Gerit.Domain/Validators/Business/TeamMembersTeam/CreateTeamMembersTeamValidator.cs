using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.TeamMembersTeam;

public class CreateTeamMembersTeamValidator : AbstractValidator<TeamMembersTeamEntity>
{
    public CreateTeamMembersTeamValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TeamId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.TeamMembersTeam.TeamIdGreaterThanZero"));
        RuleFor(x => x.TeamMemberId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.TeamMembersTeam.TeamMemberIdGreaterThanZero"));
    }
}
