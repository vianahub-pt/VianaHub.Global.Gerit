using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Billing.TenantContact;

/// <summary>
/// Validador completo para TenantContactPersonsEntity
/// </summary>
public class TenantContactValidator : BaseEntityValidator<TenantContactPersonsEntity>
{
    public TenantContactValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(TenantContactPersonsEntity entity)
    {
        var validator = new CreateTenantContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(TenantContactPersonsEntity entity)
    {
        var validator = new UpdateTenantContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(TenantContactPersonsEntity entity)
    {
        var validator = new ActivateTenantContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(TenantContactPersonsEntity entity)
    {
        var validator = new DeactivateTenantContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(TenantContactPersonsEntity entity)
    {
        var validator = new DeleteTenantContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(TenantContactPersonsEntity entity)
    {
        // Não aplicável para TenantContact
        return Task.FromResult(new ValidationResult());
    }
}
