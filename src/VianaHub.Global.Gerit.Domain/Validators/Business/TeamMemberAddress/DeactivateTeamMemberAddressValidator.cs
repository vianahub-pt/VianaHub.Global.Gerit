using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.TeamMemberAddress;

/// <summary>
/// Validador para desativação de TeamMemberAddress
/// </summary>
public class DeactivateTeamMemberAddressValidator : AbstractValidator<TeamMemberAddressEntity>
{
    private readonly ILocalizationService _localization;

    public DeactivateTeamMemberAddressValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberAddress.IdRequired"));

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberAddress.ModifiedByRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberAddress.CannotDeactivateDeleted"));
    }
}
