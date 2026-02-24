using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.InterventionTeamEquipment;

public class UpdateInterventionTeamEquipmentValidator : AbstractValidator<InterventionTeamEquipmentEntity>
{
    public UpdateInterventionTeamEquipmentValidator(ILocalizationService localization)
    {
        RuleFor(x => x.InterventionTeamId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.InterventionTeamEquipment.InterventionTeamIdGreaterThanZero"));
        RuleFor(x => x.EquipmentId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.InterventionTeamEquipment.EquipmentIdGreaterThanZero"));
    }
}
