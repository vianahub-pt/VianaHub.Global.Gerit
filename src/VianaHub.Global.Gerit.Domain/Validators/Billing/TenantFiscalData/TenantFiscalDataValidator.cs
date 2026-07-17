using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Billing.TenantFiscalData;

/// <summary>
/// Validador completo para TenantFiscalDataEntity
/// </summary>
public class TenantFiscalDataValidator : BaseEntityValidator<TenantFiscalDataEntity>
{
    public TenantFiscalDataValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(TenantFiscalDataEntity entity)
    {
        var validator = new CreateTenantFiscalDataValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(TenantFiscalDataEntity entity)
    {
        var validator = new UpdateTenantFiscalDataValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(TenantFiscalDataEntity entity)
    {
        var validator = new ActivateTenantFiscalDataValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(TenantFiscalDataEntity entity)
    {
        var validator = new DeactivateTenantFiscalDataValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(TenantFiscalDataEntity entity)
    {
        var validator = new DeleteTenantFiscalDataValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(TenantFiscalDataEntity entity)
    {
        // Não aplicável para TenantFiscalData
        return Task.FromResult(new ValidationResult());
    }
}
