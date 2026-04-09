using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.EmployeeTeam;

public class UpdateEmployeeTeamValidator : AbstractValidator<EmployeeTeamEntity>
{
    public UpdateEmployeeTeamValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TeamId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.EmployeeTeam.TeamIdGreaterThanZero"));
        RuleFor(x => x.EmployeeId).GreaterThan(0).WithMessage(localization.GetMessage("Domain.EmployeeTeam.EmployeeIdGreaterThanZero"));
    }
}
