using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.EmployeeTeam;

public class ActivateEmployeeTeamValidator : AbstractValidator<EmployeeTeamEntity>
{
    public ActivateEmployeeTeamValidator(ILocalizationService localization)
    {
        // no specific rules
    }
}
