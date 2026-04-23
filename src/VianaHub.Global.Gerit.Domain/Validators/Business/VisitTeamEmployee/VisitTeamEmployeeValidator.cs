using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitTeamEmployee;

public class VisitTeamEmployeeValidator : IEntityDomainValidator<VisitTeamEmployeeEntity>
{
    public async Task<ValidationResult> ValidateForCreateAsync(VisitTeamEmployeeEntity entity)
    {
        return await ValidateAsync(entity);
    }

    public async Task<ValidationResult> ValidateForUpdateAsync(VisitTeamEmployeeEntity entity)
    {
        return await ValidateAsync(entity);
    }

    public async Task<ValidationResult> ValidateForActivateAsync(VisitTeamEmployeeEntity entity)
    {
        if (entity == null)
            return new ValidationResult(new[] { new ValidationFailure("Entity", "visit_team_employee.entity.null") });

        return await Task.FromResult(new ValidationResult());
    }

    public async Task<ValidationResult> ValidateForDeactivateAsync(VisitTeamEmployeeEntity entity)
    {
        if (entity == null)
            return new ValidationResult(new[] { new ValidationFailure("Entity", "visit_team_employee.entity.null") });

        return await Task.FromResult(new ValidationResult());
    }

    public async Task<ValidationResult> ValidateForDeleteAsync(VisitTeamEmployeeEntity entity)
    {
        if (entity == null)
            return new ValidationResult(new[] { new ValidationFailure("Entity", "visit_team_employee.entity.null") });

        return await Task.FromResult(new ValidationResult());
    }

    public async Task<ValidationResult> ValidateForRevokeAsync(VisitTeamEmployeeEntity entity)
    {
        if (entity == null)
            return new ValidationResult(new[] { new ValidationFailure("Entity", "visit_team_employee.entity.null") });

        return await Task.FromResult(new ValidationResult());
    }

    private async Task<ValidationResult> ValidateAsync(VisitTeamEmployeeEntity entity)
    {
        var errors = new List<ValidationFailure>();

        if (entity == null)
        {
            errors.Add(new ValidationFailure("Entity", "visit_team_employee.entity.null"));
            return new ValidationResult(errors);
        }

        if (entity.TenantId <= 0)
            errors.Add(new ValidationFailure(nameof(entity.TenantId), "visit_team_employee.tenant_id.invalid"));

        if (entity.VisitTeamId <= 0)
            errors.Add(new ValidationFailure(nameof(entity.VisitTeamId), "visit_team_employee.visit_team_id.invalid"));

        if (entity.EmployeeId <= 0)
            errors.Add(new ValidationFailure(nameof(entity.EmployeeId), "visit_team_employee.employee_id.invalid"));

        if (entity.FunctionId <= 0)
            errors.Add(new ValidationFailure(nameof(entity.FunctionId), "visit_team_employee.function_id.invalid"));

        if (entity.StartDateTime == default)
            errors.Add(new ValidationFailure(nameof(entity.StartDateTime), "visit_team_employee.start_date_time.required"));

        if (entity.EndDateTime.HasValue && entity.EndDateTime.Value < entity.StartDateTime)
            errors.Add(new ValidationFailure(nameof(entity.EndDateTime), "visit_team_employee.end_date_time.must_be_after_start"));

        return await Task.FromResult(new ValidationResult(errors));
    }
}
