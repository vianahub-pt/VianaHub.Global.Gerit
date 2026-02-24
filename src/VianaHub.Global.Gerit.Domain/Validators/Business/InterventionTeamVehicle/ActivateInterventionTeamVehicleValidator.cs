using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.InterventionTeamVehicle;

public class ActivateInterventionTeamVehicleValidator : AbstractValidator<InterventionTeamVehicleEntity>
{
    public ActivateInterventionTeamVehicleValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage(localization.GetMessage("Domain.InterventionTeamVehicle.IdRequired"));
        RuleFor(x => x.ModifiedBy).GreaterThan(0).WithMessage(localization.GetMessage("Domain.InterventionTeamVehicle.ModifiedByRequired"));
        RuleFor(x => x.IsDeleted).Equal(false).WithMessage(localization.GetMessage("Domain.InterventionTeamVehicle.IsDeleted"));
    }
}
