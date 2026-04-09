using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitTeamEquipment;

public class DeleteVisitTeamEquipmentValidator : AbstractValidator<VisitTeamEquipmentEntity>
{
    public DeleteVisitTeamEquipmentValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage(localization.GetMessage("Domain.VisitTeamEquipment.IdGreaterThanZero"));
    }
}
