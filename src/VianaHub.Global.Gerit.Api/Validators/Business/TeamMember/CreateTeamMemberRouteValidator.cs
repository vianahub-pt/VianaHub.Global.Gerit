using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.TeamMember;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.TeamMember;

public class CreateTeamMemberRouteValidator : AbstractValidator<CreateTeamMemberRequest>
{
    public CreateTeamMemberRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.FunctionId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.TeamMember.Create.FunctionId"));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.TeamMember.Create.Name"))
            .MaximumLength(150).WithMessage(localization.GetMessage("Api.Validator.TeamMember.Create.Name.MaximumLength", 150));

        RuleFor(x => x.TaxNumber)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.TeamMember.Create.TaxNumber"))
            .MaximumLength(20).WithMessage(localization.GetMessage("Api.Validator.TeamMember.Create.TaxNumber.MaximumLength", 20));
    }
}
