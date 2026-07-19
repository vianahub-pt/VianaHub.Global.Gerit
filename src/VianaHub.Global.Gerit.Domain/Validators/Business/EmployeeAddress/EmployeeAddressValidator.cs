using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.EmployeeAddress;

/// <summary>
/// Validador completo para EmployeeAddressesEntity
/// </summary>
public class EmployeeAddressValidator : BaseEntityValidator<EmployeeAddressesEntity>
{
    public EmployeeAddressValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(EmployeeAddressesEntity entity)
    {
        var validator = new CreateEmployeeAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(EmployeeAddressesEntity entity)
    {
        var validator = new UpdateEmployeeAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(EmployeeAddressesEntity entity)
    {
        var validator = new ActivateEmployeeAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(EmployeeAddressesEntity entity)
    {
        var validator = new DeactivateEmployeeAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(EmployeeAddressesEntity entity)
    {
        var validator = new DeleteEmployeeAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(EmployeeAddressesEntity entity)
    {
        // EmployeeAddress n�o tem opera��o de revoke
        return Task.FromResult(new ValidationResult());
    }
}
