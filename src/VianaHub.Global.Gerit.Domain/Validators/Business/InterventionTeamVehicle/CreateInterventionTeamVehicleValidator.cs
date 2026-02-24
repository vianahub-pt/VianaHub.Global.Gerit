using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.InterventionTeamVehicle;

public class CreateInterventionTeamVehicleValidator : AbstractValidator<InterventionTeamVehicleEntity>
{
    public CreateInterventionTeamVehicleValidator(ILocalizationService localization)
    {
        RuleFor(x => x.InterventionId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.InterventionTeamVehicle.InterventionIdGreaterThanZero"));
        RuleFor(x => x.VehicleId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.InterventionTeamVehicle.VehicleIdGreaterThanZero"));
    }
}
