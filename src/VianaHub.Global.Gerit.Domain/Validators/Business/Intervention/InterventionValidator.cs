using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.Intervention;

public class InterventionValidator : BaseEntityValidator<InterventionEntity>
{
    private readonly ILocalizationService _localization;

    public InterventionValidator(ILocalizationService localization) : base(localization)
    {
        _localization = localization;
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(InterventionEntity entity)
    {
        var validator = new CreateInterventionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(InterventionEntity entity)
    {
        var validator = new UpdateInterventionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(InterventionEntity entity)
    {
        var validator = new ActivateInterventionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(InterventionEntity entity)
    {
        var validator = new DeactivateInterventionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(InterventionEntity entity)
    {
        var validator = new DeleteInterventionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(InterventionEntity entity)
    {
        return Task.FromResult(new ValidationResult());
    }
}
