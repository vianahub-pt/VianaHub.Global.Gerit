using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitTeamEquipment;

public class UpdateVisitTeamEquipmentValidator : AbstractValidator<VisitTeamEquipmentEntity>
{
    public UpdateVisitTeamEquipmentValidator(ILocalizationService localization)
    {
        RuleFor(x => x.VisitTeamId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.VisitTeamEquipment.VisitTeamIdGreaterThanZero"));
        RuleFor(x => x.EquipmentId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.VisitTeamEquipment.EquipmentIdGreaterThanZero"));
    }
}
