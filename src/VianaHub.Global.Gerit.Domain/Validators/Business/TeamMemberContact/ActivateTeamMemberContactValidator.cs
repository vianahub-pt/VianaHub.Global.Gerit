using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.TeamMemberContact;

/// <summary>
/// Validator para ativação de TeamMemberContact
/// </summary>
public class ActivateTeamMemberContactValidator : AbstractValidator<TeamMemberContactEntity>
{
    private readonly ILocalizationService _localization;

    public ActivateTeamMemberContactValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberContact.IdRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberContact.CannotActivateDeleted"));

        RuleFor(x => x.IsActive)
            .Equal(false)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberContact.AlreadyActive"));

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberContact.ModifiedByRequired"));
    }
}
