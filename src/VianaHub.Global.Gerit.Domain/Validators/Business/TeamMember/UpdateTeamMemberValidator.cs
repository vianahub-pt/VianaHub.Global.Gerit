using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.TeamMember;

public class UpdateTeamMemberValidator : AbstractValidator<TeamMemberEntity>
{
    public UpdateTeamMemberValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.TeamMember.IdRequired"));

        RuleFor(x => x.FunctionId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.TeamMember.FunctionIdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.TeamMember.NameRequired"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Domain.TeamMember.NameMaxLength", 150));
    }
}
