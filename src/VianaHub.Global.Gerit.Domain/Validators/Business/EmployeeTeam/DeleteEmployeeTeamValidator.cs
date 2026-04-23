using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.EmployeeTeam;

public class DeleteEmployeeTeamValidator : AbstractValidator<EmployeeTeamEntity>
{
    public DeleteEmployeeTeamValidator(ILocalizationService localization)
    {
        // no specific rules
    }
}
