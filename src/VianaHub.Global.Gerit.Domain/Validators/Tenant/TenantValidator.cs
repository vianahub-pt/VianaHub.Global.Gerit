using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Tenant;

/// <summary>
/// Validador completo para TenantEntity
/// </summary>
public class TenantValidator : BaseEntityValidator<TenantEntity>
{
    public TenantValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(TenantEntity entity)
    {
        var validator = new CreateTenantValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(TenantEntity entity)
    {
        var validator = new UpdateTenantValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(TenantEntity entity)
    {
        var validator = new ActivateTenantValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(TenantEntity entity)
    {
        var validator = new DeactivateTenantValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(TenantEntity entity)
    {
        var validator = new DeleteTenantValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(TenantEntity entity)
    {
        // Tenants não têm operação de revoke
        return Task.FromResult(new ValidationResult());
    }
}
