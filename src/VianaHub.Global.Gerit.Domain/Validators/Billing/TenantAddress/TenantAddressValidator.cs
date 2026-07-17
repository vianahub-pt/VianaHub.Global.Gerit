using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Billing.TenantAddress;

/// <summary>
/// Validador completo para TenantAddressesEntity
/// </summary>
public class TenantAddressValidator : BaseEntityValidator<TenantAddressesEntity>
{
    public TenantAddressValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(TenantAddressesEntity entity)
    {
        var validator = new CreateTenantAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(TenantAddressesEntity entity)
    {
        var validator = new UpdateTenantAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(TenantAddressesEntity entity)
    {
        var validator = new ActivateTenantAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(TenantAddressesEntity entity)
    {
        var validator = new DeactivateTenantAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(TenantAddressesEntity entity)
    {
        var validator = new DeleteTenantAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(TenantAddressesEntity entity)
    {
        // Não aplicável para TenantAddress
        return Task.FromResult(new ValidationResult());
    }
}
