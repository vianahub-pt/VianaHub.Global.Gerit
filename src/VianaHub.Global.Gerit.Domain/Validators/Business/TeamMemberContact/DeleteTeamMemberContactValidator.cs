using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.TeamMemberContact;

/// <summary>
/// Validator para exclusão de TeamMemberContact
/// </summary>
public class DeleteTeamMemberContactValidator : AbstractValidator<TeamMemberContactEntity>
{
    private readonly ILocalizationService _localization;

    public DeleteTeamMemberContactValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberContact.IdRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberContact.AlreadyDeleted"));

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberContact.ModifiedByRequired"));
    }
}
