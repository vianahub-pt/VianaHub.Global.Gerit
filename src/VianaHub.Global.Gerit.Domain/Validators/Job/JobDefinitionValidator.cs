using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Job;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Job;

public class JobDefinitionValidator : BaseEntityValidator<JobDefinitionsEntity>
{
    public JobDefinitionValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(JobDefinitionsEntity entity)
    {
        var validator = new JobDefinitionCreateValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(JobDefinitionsEntity entity)
    {
        var validator = new JobDefinitionUpdateValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(JobDefinitionsEntity entity)
    {
        // Validar exist�ncia de Cron quando recorrent
        var validator = new JobDefinitionUpdateValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(JobDefinitionsEntity entity)
    {
        // Sem valida��es adicionais para desativa��o
        return await Task.FromResult(new ValidationResult());
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(JobDefinitionsEntity entity)
    {
        if (entity.IsSystemJob)
        {
            var vr = new ValidationResult();
            vr.Errors.Add(new FluentValidation.Results.ValidationFailure("IsSystemJob", _localization.GetMessage("Domain.Job.CannotDeleteSystemJob")));
            return await Task.FromResult(vr);
        }

        return await Task.FromResult(new ValidationResult());
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(JobDefinitionsEntity entity)
    {
        return Task.FromResult(new ValidationResult());
    }
}
