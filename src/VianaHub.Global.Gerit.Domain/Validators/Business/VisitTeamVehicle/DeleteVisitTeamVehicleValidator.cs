using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitTeamVehicle;

public class DeleteVisitTeamVehicleValidator : AbstractValidator<VisitTeamVehicleEntity>
{
    public DeleteVisitTeamVehicleValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage(localization.GetMessage("Domain.VisitTeamVehicle.IdRequired"));
        RuleFor(x => x.ModifiedBy).GreaterThan(0).WithMessage(localization.GetMessage("Domain.VisitTeamVehicle.ModifiedByRequired"));
    }
}
