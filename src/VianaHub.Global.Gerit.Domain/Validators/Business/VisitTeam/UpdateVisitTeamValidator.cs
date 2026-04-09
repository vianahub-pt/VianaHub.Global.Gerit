using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitTeam;

public class UpdateVisitTeamValidator : AbstractValidator<VisitTeamEntity>
{
    public UpdateVisitTeamValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage(localization.GetMessage("Domain.VisitTeam.IdRequired"));
        RuleFor(x => x.VisitId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.VisitTeam.VisitIdGreaterThanZero"));
        RuleFor(x => x.TeamId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.VisitTeam.TeamIdGreaterThanZero"));
        RuleFor(x => x.ModifiedBy).GreaterThan(0).WithMessage(localization.GetMessage("Domain.VisitTeam.ModifiedByRequired"));
    }
}
