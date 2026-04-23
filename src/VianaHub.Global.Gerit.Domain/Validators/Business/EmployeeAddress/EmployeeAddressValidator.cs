using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.EmployeeAddress;

/// <summary>
/// Validador completo para EmployeeAddressEntity
/// </summary>
public class EmployeeAddressValidator : BaseEntityValidator<EmployeeAddressEntity>
{
    public EmployeeAddressValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(EmployeeAddressEntity entity)
    {
        var validator = new CreateEmployeeAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(EmployeeAddressEntity entity)
    {
        var validator = new UpdateEmployeeAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(EmployeeAddressEntity entity)
    {
        var validator = new ActivateEmployeeAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(EmployeeAddressEntity entity)
    {
        var validator = new DeactivateEmployeeAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(EmployeeAddressEntity entity)
    {
        var validator = new DeleteEmployeeAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(EmployeeAddressEntity entity)
    {
        // EmployeeAddress n�o tem opera��o de revoke
        return Task.FromResult(new ValidationResult());
    }
}
