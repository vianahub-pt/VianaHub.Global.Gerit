using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitTeamVehicles;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.VisitTeamVehicles;

public class CreateVisitTeamVehicleRouteValidator : AbstractValidator<CreateVisitTeamVehicleRequest>
{
    public CreateVisitTeamVehicleRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.VisitTeamId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.VisitTeamVehicle.Create.VisitTeamId"));

        RuleFor(x => x.VehicleId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.VisitTeamVehicle.Create.VehicleId"));
    }
}
