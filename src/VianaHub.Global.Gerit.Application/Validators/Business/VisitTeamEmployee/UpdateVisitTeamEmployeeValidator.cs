using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitTeamEmployee;

namespace VianaHub.Global.Gerit.Application.Validators.Business.VisitTeamEmployee;

public class UpdateVisitTeamEmployeeValidator : AbstractValidator<UpdateVisitTeamEmployeeRequest>
{
    public UpdateVisitTeamEmployeeValidator()
    {
        RuleFor(x => x.FunctionId)
            .GreaterThan(0)
            .WithMessage("visit_team_employee.function_id.required");

        RuleFor(x => x.StartDateTime)
            .NotEmpty()
            .WithMessage("visit_team_employee.start_date_time.required");

        RuleFor(x => x.EndDateTime)
            .GreaterThanOrEqualTo(x => x.StartDateTime)
            .When(x => x.EndDateTime.HasValue)
            .WithMessage("visit_team_employee.end_date_time.must_be_after_start");
    }
}
