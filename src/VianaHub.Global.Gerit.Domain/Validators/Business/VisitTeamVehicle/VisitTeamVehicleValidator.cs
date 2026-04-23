using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitTeamVehicle;

public class VisitTeamVehicleValidator : BaseEntityValidator<VisitTeamVehicleEntity>
{
    public VisitTeamVehicleValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override Task<ValidationResult> ValidateForCreateAsync(VisitTeamVehicleEntity entity)
    {
        var validator = new CreateVisitTeamVehicleValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForUpdateAsync(VisitTeamVehicleEntity entity)
    {
        var validator = new UpdateVisitTeamVehicleValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForActivateAsync(VisitTeamVehicleEntity entity)
    {
        var validator = new ActivateVisitTeamVehicleValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForDeactivateAsync(VisitTeamVehicleEntity entity)
    {
        var validator = new DeactivateVisitTeamVehicleValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForDeleteAsync(VisitTeamVehicleEntity entity)
    {
        var validator = new DeleteVisitTeamVehicleValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(VisitTeamVehicleEntity entity)
    {
        return Task.FromResult(new ValidationResult());
    }
}
