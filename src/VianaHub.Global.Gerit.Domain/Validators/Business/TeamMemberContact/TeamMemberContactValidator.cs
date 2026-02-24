using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.TeamMemberContact;

/// <summary>
/// Validador completo para TeamMemberContactEntity
/// </summary>
public class TeamMemberContactValidator : BaseEntityValidator<TeamMemberContactEntity>
{
    public TeamMemberContactValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(TeamMemberContactEntity entity)
    {
        var validator = new CreateTeamMemberContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(TeamMemberContactEntity entity)
    {
        var validator = new UpdateTeamMemberContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(TeamMemberContactEntity entity)
    {
        var validator = new ActivateTeamMemberContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(TeamMemberContactEntity entity)
    {
        var validator = new DeactivateTeamMemberContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(TeamMemberContactEntity entity)
    {
        var validator = new DeleteTeamMemberContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(TeamMemberContactEntity entity)
    {
        // TeamMemberContact não tem operação de revoke
        return Task.FromResult(new ValidationResult());
    }
}
