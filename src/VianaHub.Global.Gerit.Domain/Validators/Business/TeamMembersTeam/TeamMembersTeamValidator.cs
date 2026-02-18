using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.TeamMembersTeam;

public class TeamMembersTeamValidator : BaseEntityValidator<TeamMembersTeamEntity>
{
    public TeamMembersTeamValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override Task<ValidationResult> ValidateForCreateAsync(TeamMembersTeamEntity entity)
    {
        var validator = new CreateTeamMembersTeamValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForUpdateAsync(TeamMembersTeamEntity entity)
    {
        var validator = new UpdateTeamMembersTeamValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForActivateAsync(TeamMembersTeamEntity entity)
    {
        var validator = new ActivateTeamMembersTeamValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForDeactivateAsync(TeamMembersTeamEntity entity)
    {
        var validator = new DeactivateTeamMembersTeamValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForDeleteAsync(TeamMembersTeamEntity entity)
    {
        var validator = new DeleteTeamMembersTeamValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(TeamMembersTeamEntity entity)
    {
        // TeamMembersTeam doesn't support revoke
        return Task.FromResult(new ValidationResult());
    }
}
