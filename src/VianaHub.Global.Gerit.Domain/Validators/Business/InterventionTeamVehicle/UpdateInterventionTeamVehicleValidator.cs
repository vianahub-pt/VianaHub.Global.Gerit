using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.InterventionTeamVehicle;

public class UpdateInterventionTeamVehicleValidator : AbstractValidator<InterventionTeamVehicleEntity>
{
    public UpdateInterventionTeamVehicleValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage(localization.GetMessage("Domain.InterventionTeamVehicle.IdRequired"));
        RuleFor(x => x.InterventionTeamId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.InterventionTeamVehicle.InterventionTeamIdGreaterThanZero"));
        RuleFor(x => x.VehicleId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.InterventionTeamVehicle.VehicleIdGreaterThanZero"));
        RuleFor(x => x.ModifiedBy).GreaterThan(0).WithMessage(localization.GetMessage("Domain.InterventionTeamVehicle.ModifiedByRequired"));
    }
}
