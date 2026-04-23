using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitTeam;

public class ActivateVisitTeamValidator : AbstractValidator<VisitTeamEntity>
{
    public ActivateVisitTeamValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage(localization.GetMessage("Domain.VisitTeam.IdRequired"));
        RuleFor(x => x.ModifiedBy).GreaterThan(0).WithMessage(localization.GetMessage("Domain.VisitTeam.ModifiedByRequired"));
        RuleFor(x => x.IsDeleted).Equal(false).WithMessage(localization.GetMessage("Domain.VisitTeam.IsDeleted"));
    }
}
