using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitTeamVehicle;

public class CreateVisitTeamVehicleValidator : AbstractValidator<VisitTeamVehicleEntity>
{
    public CreateVisitTeamVehicleValidator(ILocalizationService localization)
    {
        RuleFor(x => x.VisitTeamId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.VisitTeamVehicle.VisitTeamIdGreaterThanZero"));
        RuleFor(x => x.VehicleId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.VisitTeamVehicle.VehicleIdGreaterThanZero"));
    }
}
