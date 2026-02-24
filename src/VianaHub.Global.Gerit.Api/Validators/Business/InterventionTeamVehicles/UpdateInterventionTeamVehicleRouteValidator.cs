using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionTeamVehicles;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.InterventionTeamVehicles;

public class UpdateInterventionTeamVehicleRouteValidator : AbstractValidator<UpdateInterventionTeamVehicleRequest>
{
    public UpdateInterventionTeamVehicleRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.InterventionTeamId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.InterventionTeamVehicle.Update.InterventionTeamId"));

        RuleFor(x => x.VehicleId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.InterventionTeamVehicle.Update.VehicleId"));
    }
}
