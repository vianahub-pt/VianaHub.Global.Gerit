using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Vehicle;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.Vehicle;

public class UpdateVehicleRouteValidator : AbstractValidator<UpdateVehicleRequest>
{
    public UpdateVehicleRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.StatusId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Api.Validator.Vehicle.Update.StatusIdRequired"));

        RuleFor(x => x.Plate)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.Vehicle.Update.Plate"))
            .MaximumLength(20).WithMessage(localization.GetMessage("Api.Validator.Vehicle.Update.Plate.MaximumLength", 20));

        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.Vehicle.Update.Brand"))
            .MaximumLength(100).WithMessage(localization.GetMessage("Api.Validator.Vehicle.Update.Brand.MaximumLength", 100));

        RuleFor(x => x.Model)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.Vehicle.Update.Model"))
            .MaximumLength(100).WithMessage(localization.GetMessage("Api.Validator.Vehicle.Update.Model.MaximumLength", 100));

        RuleFor(x => x.Year)
            .GreaterThan(1885).WithMessage(localization.GetMessage("Api.Validator.Vehicle.Update.Year"));

        RuleFor(x => x.Color)
            .MaximumLength(50).WithMessage(localization.GetMessage("Api.Validator.Vehicle.Update.Color.MaximumLength", 50));

        RuleFor(x => x.FuelType)
            .MaximumLength(50).WithMessage(localization.GetMessage("Api.Validator.Vehicle.Update.FuelType.MaximumLength", 50));
    }
}
