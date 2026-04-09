using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.EmployeeTeams;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.EmployeeTeams;

public class CreateEmployeeTeamRouteValidator : AbstractValidator<CreateEmployeeTeamRequest>
{
    public CreateEmployeeTeamRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TeamId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.EmployeeTeam.Create.TeamId"));

        RuleFor(x => x.EmployeeId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.EmployeeTeam.Create.EmployeeId"));
    }
}
