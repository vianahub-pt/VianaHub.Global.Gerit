using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.InterventionStatus;

/// <summary>
/// Validador completo para InterventionStatusEntity
/// </summary>
public class InterventionStatusValidator : BaseEntityValidator<InterventionStatusEntity>
{
    public InterventionStatusValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(InterventionStatusEntity entity)
    {
        var validator = new CreateInterventionStatusValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(InterventionStatusEntity entity)
    {
        var validator = new UpdateInterventionStatusValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(InterventionStatusEntity entity)
    {
        var validator = new ActivateInterventionStatusValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(InterventionStatusEntity entity)
    {
        var validator = new DeactivateInterventionStatusValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(InterventionStatusEntity entity)
    {
        var validator = new DeleteInterventionStatusValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(InterventionStatusEntity entity)
    {
        // Não aplicável para InterventionStatus
        return Task.FromResult(new ValidationResult());
    }
}
