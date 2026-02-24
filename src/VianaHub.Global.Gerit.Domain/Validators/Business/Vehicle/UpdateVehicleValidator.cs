using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.Vehicle;

public class UpdateVehicleValidator : AbstractValidator<VehicleEntity>
{
    public UpdateVehicleValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Vehicle.InvalidId"));

        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Vehicle.TenantIdRequired"));

        RuleFor(x => x.StatusId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Vehicle.StatusIdRequired"));

        RuleFor(x => x.Plate)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Vehicle.PlateRequired"))
            .MaximumLength(20)
            .WithMessage(localization.GetMessage("Domain.Vehicle.PlateMaxLength", 20));

        RuleFor(x => x.Model)
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.Vehicle.ModelMaxLength", 100));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.Vehicle.CannotUpdateDeleted"));
    }
}
