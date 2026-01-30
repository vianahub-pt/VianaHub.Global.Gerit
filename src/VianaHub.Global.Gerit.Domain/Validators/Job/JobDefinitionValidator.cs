using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Job;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Job;

public class JobDefinitionValidator : BaseEntityValidator<JobDefinitionEntity>
{
    public JobDefinitionValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(JobDefinitionEntity entity)
    {
        var validator = new JobDefinitionCreateValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(JobDefinitionEntity entity)
    {
        var validator = new JobDefinitionUpdateValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(JobDefinitionEntity entity)
    {
        // Validar existência de Cron quando recorrent
        var validator = new JobDefinitionUpdateValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(JobDefinitionEntity entity)
    {
        // Sem validações adicionais para desativação
        return await Task.FromResult(new ValidationResult());
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(JobDefinitionEntity entity)
    {
        if (entity.IsSystemJob)
        {
            var vr = new ValidationResult();
            vr.Errors.Add(new FluentValidation.Results.ValidationFailure("IsSystemJob", _localization.GetMessage("Domain.Job.CannotDeleteSystemJob")));
            return await Task.FromResult(vr);
        }

        return await Task.FromResult(new ValidationResult());
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(JobDefinitionEntity entity)
    {
        return Task.FromResult(new ValidationResult());
    }
}
