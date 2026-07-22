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
            return new ValidationResult(new[] { new ValidationFailure("Entity", "Domain.VisitTeamEmployee.EntityNull") });

        return await Task.FromResult(new ValidationResult());
    }

    public async Task<ValidationResult> ValidateForDeactivateAsync(VisitTeamEmployeeEntity entity)
    {
        if (entity == null)
            return new ValidationResult(new[] { new ValidationFailure("Entity", "Domain.VisitTeamEmployee.EntityNull") });

        return await Task.FromResult(new ValidationResult());
    }

    public async Task<ValidationResult> ValidateForDeleteAsync(VisitTeamEmployeeEntity entity)
    {
        if (entity == null)
            return new ValidationResult(new[] { new ValidationFailure("Entity", "Domain.VisitTeamEmployee.EntityNull") });

        return await Task.FromResult(new ValidationResult());
    }

    public async Task<ValidationResult> ValidateForRevokeAsync(VisitTeamEmployeeEntity entity)
    {
        if (entity == null)
            return new ValidationResult(new[] { new ValidationFailure("Entity", "Domain.VisitTeamEmployee.EntityNull") });

        return await Task.FromResult(new ValidationResult());
    }

    private async Task<ValidationResult> ValidateAsync(VisitTeamEmployeeEntity entity)
    {
        var errors = new List<ValidationFailure>();

        if (entity == null)
        {
            errors.Add(new ValidationFailure("Entity", "Domain.VisitTeamEmployee.EntityNull"));
            return new ValidationResult(errors);
        }

        if (entity.TenantId <= 0)
            errors.Add(new ValidationFailure(nameof(entity.TenantId), "Domain.VisitTeamEmployee.TenantIdInvalid"));

        if (entity.VisitTeamId <= 0)
            errors.Add(new ValidationFailure(nameof(entity.VisitTeamId), "Domain.VisitTeamEmployee.VisitTeamIdInvalid"));

        if (entity.EmployeeId <= 0)
            errors.Add(new ValidationFailure(nameof(entity.EmployeeId), "Domain.VisitTeamEmployee.EmployeeIdInvalid"));

        if (entity.VisitTeamFunctionId <= 0)
            errors.Add(new ValidationFailure(nameof(entity.VisitTeamFunctionId), "Domain.VisitTeamEmployee.FunctionIdInvalid"));

        if (entity.StartDateTime == default)
            errors.Add(new ValidationFailure(nameof(entity.StartDateTime), "Domain.VisitTeamEmployee.StartDateTimeRequired"));

        if (entity.EndDateTime.HasValue && entity.EndDateTime.Value < entity.StartDateTime)
            errors.Add(new ValidationFailure(nameof(entity.EndDateTime), "Domain.VisitTeamEmployee.EndDateTimeMustBeAfterStart"));

        return await Task.FromResult(new ValidationResult(errors));
    }
}
