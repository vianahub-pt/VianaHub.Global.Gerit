using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Billing.TenantContactPerson;

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
        var validator = new CreateTenantContactPersonValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(TenantContactPersonsEntity entity)
    {
        var validator = new UpdateTenantContactPersonValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(TenantContactPersonsEntity entity)
    {
        var validator = new ActivateTenantContactPersonValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(TenantContactPersonsEntity entity)
    {
        var validator = new DeactivateTenantContactPersonValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(TenantContactPersonsEntity entity)
    {
        var validator = new DeleteTenantContactPersonValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(TenantContactPersonsEntity entity)
    {
        // Não aplicável para TenantContact
        return Task.FromResult(new ValidationResult());
    }
}
