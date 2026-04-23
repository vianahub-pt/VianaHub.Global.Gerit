using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitTeamVehicles;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.VisitTeamVehicles;

public class UpdateVisitTeamVehicleRouteValidator : AbstractValidator<UpdateVisitTeamVehicleRequest>
{
    public UpdateVisitTeamVehicleRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.VisitTeamId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.VisitTeamVehicle.Update.VisitTeamId"));

        RuleFor(x => x.VehicleId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.VisitTeamVehicle.Update.VehicleId"));
    }
}
