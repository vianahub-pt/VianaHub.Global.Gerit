using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Identity;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Identity.Role;

/// <summary>
/// Validador completo para RoleEntity
/// </summary>
public class RoleValidator : BaseEntityValidator<RoleEntity>
{
    public RoleValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(RoleEntity entity)
    {
        var validator = new CreateRoleValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(RoleEntity entity)
    {
        var validator = new UpdateRoleValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(RoleEntity entity)
    {
        var validator = new ActivateRoleValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(RoleEntity entity)
    {
        var validator = new DeactivateRoleValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(RoleEntity entity)
    {
        var validator = new DeleteRoleValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(RoleEntity entity)
    {
        // Roles não têm operação de revoke
        return Task.FromResult(new ValidationResult());
    }
}
