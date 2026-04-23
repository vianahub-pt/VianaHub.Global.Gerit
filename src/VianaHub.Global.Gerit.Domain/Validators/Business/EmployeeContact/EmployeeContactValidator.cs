using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.EmployeeContact;

/// <summary>
/// Validador completo para EmployeeContactEntity
/// </summary>
public class EmployeeContactValidator : BaseEntityValidator<EmployeeContactEntity>
{
    public EmployeeContactValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(EmployeeContactEntity entity)
    {
        var validator = new CreateEmployeeContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(EmployeeContactEntity entity)
    {
        var validator = new UpdateEmployeeContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(EmployeeContactEntity entity)
    {
        var validator = new ActivateEmployeeContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(EmployeeContactEntity entity)
    {
        var validator = new DeactivateEmployeeContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(EmployeeContactEntity entity)
    {
        var validator = new DeleteEmployeeContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(EmployeeContactEntity entity)
    {
        // EmployeeContact n�o tem opera��o de revoke
        return Task.FromResult(new ValidationResult());
    }
}
