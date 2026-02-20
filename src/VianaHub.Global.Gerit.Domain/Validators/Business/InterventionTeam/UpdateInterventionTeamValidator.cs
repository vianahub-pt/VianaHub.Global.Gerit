using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.InterventionTeam;

public class UpdateInterventionTeamValidator : AbstractValidator<InterventionTeamEntity>
{
    public UpdateInterventionTeamValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage(localization.GetMessage("Domain.InterventionTeam.IdRequired"));
        RuleFor(x => x.InterventionId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.InterventionTeam.InterventionIdGreaterThanZero"));
        RuleFor(x => x.TeamId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.InterventionTeam.TeamIdGreaterThanZero"));
        RuleFor(x => x.ModifiedBy).GreaterThan(0).WithMessage(localization.GetMessage("Domain.InterventionTeam.ModifiedByRequired"));
    }
}
