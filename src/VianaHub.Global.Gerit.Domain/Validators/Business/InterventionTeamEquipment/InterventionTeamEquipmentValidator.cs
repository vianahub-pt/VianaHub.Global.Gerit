using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.InterventionTeamEquipment;

public class InterventionTeamEquipmentValidator : BaseEntityValidator<InterventionTeamEquipmentEntity>
{
    public InterventionTeamEquipmentValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override Task<ValidationResult> ValidateForCreateAsync(InterventionTeamEquipmentEntity entity)
    {
        var validator = new CreateInterventionTeamEquipmentValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForUpdateAsync(InterventionTeamEquipmentEntity entity)
    {
        var validator = new UpdateInterventionTeamEquipmentValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForActivateAsync(InterventionTeamEquipmentEntity entity)
    {
        var validator = new ActivateInterventionTeamEquipmentValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForDeactivateAsync(InterventionTeamEquipmentEntity entity)
    {
        var validator = new DeactivateInterventionTeamEquipmentValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForDeleteAsync(InterventionTeamEquipmentEntity entity)
    {
        var validator = new DeleteInterventionTeamEquipmentValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(InterventionTeamEquipmentEntity entity)
    {
        return Task.FromResult(new ValidationResult());
    }
}
