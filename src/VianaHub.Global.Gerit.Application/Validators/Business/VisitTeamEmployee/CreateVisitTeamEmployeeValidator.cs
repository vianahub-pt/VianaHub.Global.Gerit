using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitTeamEmployee;

namespace VianaHub.Global.Gerit.Application.Validators.Business.VisitTeamEmployee;

public class CreateVisitTeamEmployeeValidator : AbstractValidator<CreateVisitTeamEmployeeRequest>
{
    public CreateVisitTeamEmployeeValidator()
    {
        RuleFor(x => x.VisitTeamId)
            .GreaterThan(0)
            .WithMessage("visit_team_employee.visit_team_id.required");

        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .WithMessage("visit_team_employee.employee_id.required");

        RuleFor(x => x.FunctionId)
            .GreaterThan(0)
            .WithMessage("visit_team_employee.function_id.required");

        RuleFor(x => x.StartDateTime)
            .NotEmpty()
            .WithMessage("visit_team_employee.start_date_time.required")
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(365))
            .WithMessage("visit_team_employee.start_date_time.invalid_future");
    }
}
