using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.InterventionTeamEquipment;

public class DeleteInterventionTeamEquipmentValidator : AbstractValidator<InterventionTeamEquipmentEntity>
{
    public DeleteInterventionTeamEquipmentValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage(localization.GetMessage("Domain.InterventionTeamEquipment.IdGreaterThanZero"));
    }
}
