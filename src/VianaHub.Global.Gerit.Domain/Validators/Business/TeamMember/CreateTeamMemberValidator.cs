using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.TeamMember;

public class CreateTeamMemberValidator : AbstractValidator<TeamMemberEntity>
{
    public CreateTeamMemberValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.TeamMember.TenantIdRequired"));

        RuleFor(x => x.FunctionId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.TeamMember.FunctionIdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.TeamMember.NameRequired"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Domain.TeamMember.NameMaxLength", 150));

        RuleFor(x => x.TaxNumber)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.TeamMember.TaxNumberRequired"))
            .MaximumLength(20)
            .WithMessage(localization.GetMessage("Domain.TeamMember.TaxNumberMaxLength", 20));
    }
}
