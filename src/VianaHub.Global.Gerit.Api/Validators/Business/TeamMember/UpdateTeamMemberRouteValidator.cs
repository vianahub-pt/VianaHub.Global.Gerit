using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.TeamMember;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.TeamMember;

public class UpdateTeamMemberRouteValidator : AbstractValidator<UpdateTeamMemberRequest>
{
    public UpdateTeamMemberRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.FunctionId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.TeamMember.Update.FunctionId"));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.TeamMember.Update.Name"))
            .MaximumLength(150).WithMessage(localization.GetMessage("Api.Validator.TeamMember.Update.Name.MaximumLength", 150));
    }
}
