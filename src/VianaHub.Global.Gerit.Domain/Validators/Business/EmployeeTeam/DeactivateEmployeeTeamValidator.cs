using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.EmployeeTeam;

public class DeactivateEmployeeTeamValidator : AbstractValidator<EmployeeTeamEntity>
{
    public DeactivateEmployeeTeamValidator(ILocalizationService localization)
    {
        // no specific rules
    }
}
