using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.TeamMemberAddress;

/// <summary>
/// Validador completo para TeamMemberAddressEntity
/// </summary>
public class TeamMemberAddressValidator : BaseEntityValidator<TeamMemberAddressEntity>
{
    public TeamMemberAddressValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(TeamMemberAddressEntity entity)
    {
        var validator = new CreateTeamMemberAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(TeamMemberAddressEntity entity)
    {
        var validator = new UpdateTeamMemberAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(TeamMemberAddressEntity entity)
    {
        var validator = new ActivateTeamMemberAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(TeamMemberAddressEntity entity)
    {
        var validator = new DeactivateTeamMemberAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(TeamMemberAddressEntity entity)
    {
        var validator = new DeleteTeamMemberAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(TeamMemberAddressEntity entity)
    {
        // TeamMemberAddress não tem operação de revoke
        return Task.FromResult(new ValidationResult());
    }
}
