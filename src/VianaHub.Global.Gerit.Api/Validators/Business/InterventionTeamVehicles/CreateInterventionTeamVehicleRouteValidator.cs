using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionTeamVehicles;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.InterventionTeamVehicles;

public class CreateInterventionTeamVehicleRouteValidator : AbstractValidator<CreateInterventionTeamVehicleRequest>
{
    public CreateInterventionTeamVehicleRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.InterventionId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.InterventionTeamVehicle.Create.InterventionId"));

        RuleFor(x => x.VehicleId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.InterventionTeamVehicle.Create.VehicleId"));
    }
}
