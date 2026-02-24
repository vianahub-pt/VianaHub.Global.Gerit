using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.InterventionTeamEquipment;

public class CreateInterventionTeamEquipmentValidator : AbstractValidator<InterventionTeamEquipmentEntity>
{
    public CreateInterventionTeamEquipmentValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.InterventionTeamEquipment.TenantIdGreaterThanZero"));
        RuleFor(x => x.InterventionTeamId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.InterventionTeamEquipment.InterventionTeamIdGreaterThanZero"));
        RuleFor(x => x.EquipmentId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.InterventionTeamEquipment.EquipmentIdGreaterThanZero"));
    }
}
