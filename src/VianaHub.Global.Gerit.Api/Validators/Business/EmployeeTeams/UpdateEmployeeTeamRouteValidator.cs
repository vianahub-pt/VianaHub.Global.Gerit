using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.EmployeeTeams;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.EmployeeTeams;

public class UpdateEmployeeTeamRouteValidator : AbstractValidator<UpdateEmployeeTeamRequest>
{
    public UpdateEmployeeTeamRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TeamId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.EmployeeTeam.Update.TeamId"));

        RuleFor(x => x.EmployeeId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.EmployeeTeam.Update.EmployeeId"));
    }
}
