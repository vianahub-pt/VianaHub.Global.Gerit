using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Billing.TenantContact;

/// <summary>
/// Validador completo para TenantContactEntity
/// </summary>
public class TenantContactValidator : BaseEntityValidator<TenantContactEntity>
{
    public TenantContactValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(TenantContactEntity entity)
    {
        var validator = new CreateTenantContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(TenantContactEntity entity)
    {
        var validator = new UpdateTenantContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(TenantContactEntity entity)
    {
        var validator = new ActivateTenantContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(TenantContactEntity entity)
    {
        var validator = new DeactivateTenantContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(TenantContactEntity entity)
    {
        var validator = new DeleteTenantContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(TenantContactEntity entity)
    {
        // Não aplicável para TenantContact
        return Task.FromResult(new ValidationResult());
    }
}
