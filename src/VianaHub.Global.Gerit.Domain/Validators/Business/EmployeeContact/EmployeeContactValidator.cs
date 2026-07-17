using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.EmployeeContact;

/// <summary>
/// Validador completo para EmployeeContactPersonsEntity
/// </summary>
public class EmployeeContactValidator : BaseEntityValidator<EmployeeContactPersonsEntity>
{
    public EmployeeContactValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(EmployeeContactPersonsEntity entity)
    {
        var validator = new CreateEmployeeContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(EmployeeContactPersonsEntity entity)
    {
        var validator = new UpdateEmployeeContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(EmployeeContactPersonsEntity entity)
    {
        var validator = new ActivateEmployeeContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(EmployeeContactPersonsEntity entity)
    {
        var validator = new DeactivateEmployeeContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(EmployeeContactPersonsEntity entity)
    {
        var validator = new DeleteEmployeeContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(EmployeeContactPersonsEntity entity)
    {
        // EmployeeContact n�o tem opera��o de revoke
        return Task.FromResult(new ValidationResult());
    }
}
