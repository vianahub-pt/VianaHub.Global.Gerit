using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitTeamVehicle;

public class UpdateVisitTeamVehicleValidator : AbstractValidator<VisitTeamVehicleEntity>
{
    public UpdateVisitTeamVehicleValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage(localization.GetMessage("Domain.VisitTeamVehicle.IdRequired"));
        RuleFor(x => x.VisitTeamId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.VisitTeamVehicle.VisitTeamIdGreaterThanZero"));
        RuleFor(x => x.VehicleId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.VisitTeamVehicle.VehicleIdGreaterThanZero"));
        RuleFor(x => x.ModifiedBy).GreaterThan(0).WithMessage(localization.GetMessage("Domain.VisitTeamVehicle.ModifiedByRequired"));
    }
}
