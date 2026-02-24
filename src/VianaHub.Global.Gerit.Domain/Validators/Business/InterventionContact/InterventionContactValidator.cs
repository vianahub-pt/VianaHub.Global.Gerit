using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.InterventionContact;

/// <summary>
/// Validador completo para InterventionContactEntity
/// </summary>
public class InterventionContactValidator : BaseEntityValidator<InterventionContactEntity>
{
    public InterventionContactValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(InterventionContactEntity entity)
    {
        var validator = new CreateInterventionContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(InterventionContactEntity entity)
    {
        var validator = new UpdateInterventionContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(InterventionContactEntity entity)
    {
        var validator = new ActivateInterventionContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(InterventionContactEntity entity)
    {
        var validator = new DeactivateInterventionContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(InterventionContactEntity entity)
    {
        var validator = new DeleteInterventionContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(InterventionContactEntity entity)
    {
        // Não aplicável para InterventionContact
        return Task.FromResult(new ValidationResult());
    }
}
