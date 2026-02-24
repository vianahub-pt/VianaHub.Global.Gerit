using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.InterventionAddress;

/// <summary>
/// Validador completo para InterventionAddressEntity
/// </summary>
public class InterventionAddressValidator : BaseEntityValidator<InterventionAddressEntity>
{
    public InterventionAddressValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(InterventionAddressEntity entity)
    {
        var validator = new CreateInterventionAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(InterventionAddressEntity entity)
    {
        var validator = new UpdateInterventionAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(InterventionAddressEntity entity)
    {
        var validator = new ActivateInterventionAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(InterventionAddressEntity entity)
    {
        var validator = new DeactivateInterventionAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(InterventionAddressEntity entity)
    {
        var validator = new DeleteInterventionAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(InterventionAddressEntity entity)
    {
        return Task.FromResult(new ValidationResult());
    }
}
