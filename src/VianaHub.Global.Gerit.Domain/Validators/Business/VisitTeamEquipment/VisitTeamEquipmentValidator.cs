using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitTeamEquipment;

public class VisitTeamEquipmentValidator : BaseEntityValidator<VisitTeamEquipmentEntity>
{
    public VisitTeamEquipmentValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override Task<ValidationResult> ValidateForCreateAsync(VisitTeamEquipmentEntity entity)
    {
        var validator = new CreateVisitTeamEquipmentValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForUpdateAsync(VisitTeamEquipmentEntity entity)
    {
        var validator = new UpdateVisitTeamEquipmentValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForActivateAsync(VisitTeamEquipmentEntity entity)
    {
        var validator = new ActivateVisitTeamEquipmentValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForDeactivateAsync(VisitTeamEquipmentEntity entity)
    {
        var validator = new DeactivateVisitTeamEquipmentValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForDeleteAsync(VisitTeamEquipmentEntity entity)
    {
        var validator = new DeleteVisitTeamEquipmentValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(VisitTeamEquipmentEntity entity)
    {
        return Task.FromResult(new ValidationResult());
    }
}
