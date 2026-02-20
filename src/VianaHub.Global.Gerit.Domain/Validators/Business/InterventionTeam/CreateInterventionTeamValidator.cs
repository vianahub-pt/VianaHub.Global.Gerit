using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.InterventionTeam;

public class CreateInterventionTeamValidator : AbstractValidator<InterventionTeamEntity>
{
    public CreateInterventionTeamValidator(ILocalizationService localization)
    {
        RuleFor(x => x.InterventionId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.InterventionTeam.InterventionIdGreaterThanZero"));
        RuleFor(x => x.TeamId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.InterventionTeam.TeamIdGreaterThanZero"));
    }
}
