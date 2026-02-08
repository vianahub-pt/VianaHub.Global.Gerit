using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.TeamMemberContact;

/// <summary>
/// Validator para desativação de TeamMemberContact
/// </summary>
public class DeactivateTeamMemberContactValidator : AbstractValidator<TeamMemberContactEntity>
{
    private readonly ILocalizationService _localization;

    public DeactivateTeamMemberContactValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberContact.IdRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberContact.CannotDeactivateDeleted"));

        RuleFor(x => x.IsActive)
            .Equal(true)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberContact.AlreadyInactive"));

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberContact.ModifiedByRequired"));
    }
}
