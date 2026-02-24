using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.Status;

/// <summary>
/// Validador completo para StatusEntity
/// </summary>
public class StatusValidator : BaseEntityValidator<StatusEntity>
{
    public StatusValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(StatusEntity entity)
    {
        var validator = new CreateStatusValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(StatusEntity entity)
    {
        var validator = new UpdateStatusValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(StatusEntity entity)
    {
        var validator = new ActivateStatusValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(StatusEntity entity)
    {
        var validator = new DeactivateStatusValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(StatusEntity entity)
    {
        var validator = new DeleteStatusValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(StatusEntity entity)
    {
        // Não aplicável para Status
        return Task.FromResult(new ValidationResult());
    }
}
