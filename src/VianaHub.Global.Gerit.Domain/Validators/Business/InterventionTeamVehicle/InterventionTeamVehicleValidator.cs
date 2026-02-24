using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.InterventionTeamVehicle;

public class InterventionTeamVehicleValidator : BaseEntityValidator<InterventionTeamVehicleEntity>
{
    public InterventionTeamVehicleValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override Task<ValidationResult> ValidateForCreateAsync(InterventionTeamVehicleEntity entity)
    {
        var validator = new CreateInterventionTeamVehicleValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForUpdateAsync(InterventionTeamVehicleEntity entity)
    {
        var validator = new UpdateInterventionTeamVehicleValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForActivateAsync(InterventionTeamVehicleEntity entity)
    {
        var validator = new ActivateInterventionTeamVehicleValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForDeactivateAsync(InterventionTeamVehicleEntity entity)
    {
        var validator = new DeactivateInterventionTeamVehicleValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForDeleteAsync(InterventionTeamVehicleEntity entity)
    {
        var validator = new DeleteInterventionTeamVehicleValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(InterventionTeamVehicleEntity entity)
    {
        return Task.FromResult(new ValidationResult());
    }
}
