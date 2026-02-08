using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.TeamMemberContact;

/// <summary>
/// Validator para criação de TeamMemberContact
/// </summary>
public class CreateTeamMemberContactValidator : AbstractValidator<TeamMemberContactEntity>
{
    private readonly ILocalizationService _localization;

    public CreateTeamMemberContactValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberContact.TenantIdRequired"));

        RuleFor(x => x.TeamMemberId)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberContact.TeamMemberIdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Domain.TeamMemberContact.NameRequired"))
            .MaximumLength(150)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberContact.NameMaxLength"));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Domain.TeamMemberContact.EmailRequired"))
            .MaximumLength(255)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberContact.EmailMaxLength"))
            .EmailAddress()
            .WithMessage(_localization.GetMessage("Domain.TeamMemberContact.EmailInvalid"));

        RuleFor(x => x.Phone)
            .MaximumLength(30)
            .When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage(_localization.GetMessage("Domain.TeamMemberContact.PhoneMaxLength"));

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberContact.CreatedByRequired"));
    }
}
