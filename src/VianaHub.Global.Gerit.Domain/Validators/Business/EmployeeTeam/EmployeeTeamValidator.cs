using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.EmployeeTeam;

public class EmployeeTeamValidator : BaseEntityValidator<EmployeeTeamEntity>
{
    public EmployeeTeamValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override Task<ValidationResult> ValidateForCreateAsync(EmployeeTeamEntity entity)
    {
        var validator = new CreateEmployeeTeamValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForUpdateAsync(EmployeeTeamEntity entity)
    {
        var validator = new UpdateEmployeeTeamValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForActivateAsync(EmployeeTeamEntity entity)
    {
        var validator = new ActivateEmployeeTeamValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForDeactivateAsync(EmployeeTeamEntity entity)
    {
        var validator = new DeactivateEmployeeTeamValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForDeleteAsync(EmployeeTeamEntity entity)
    {
        var validator = new DeleteEmployeeTeamValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(EmployeeTeamEntity entity)
    {
        // EmployeeTeam doesn't support revoke
        return Task.FromResult(new ValidationResult());
    }
}
