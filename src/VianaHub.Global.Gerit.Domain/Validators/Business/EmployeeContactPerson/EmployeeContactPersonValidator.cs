using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.EmployeeContact;

/// <summary>
/// Validador completo para EmployeeContactPersonsEntity
/// </summary>
public class EmployeeContactPersonValidator : BaseEntityValidator<EmployeeContactPersonsEntity>
{
    public EmployeeContactPersonValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(EmployeeContactPersonsEntity entity)
    {
        var validator = new CreateEmployeeContactPersonValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(EmployeeContactPersonsEntity entity)
    {
        var validator = new UpdateEmployeeContactPersonValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(EmployeeContactPersonsEntity entity)
    {
        var validator = new ActivateEmployeeContactPersonValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(EmployeeContactPersonsEntity entity)
    {
        var validator = new DeactivateEmployeeContactPersonValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(EmployeeContactPersonsEntity entity)
    {
        var validator = new DeleteEmployeeContactPersonValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(EmployeeContactPersonsEntity entity)
    {
        // EmployeeContact n�o tem opera��o de revoke
        return Task.FromResult(new ValidationResult());
    }
}
