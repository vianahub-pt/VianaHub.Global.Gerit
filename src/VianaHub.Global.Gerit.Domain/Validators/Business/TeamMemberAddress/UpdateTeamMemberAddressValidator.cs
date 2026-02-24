using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.TeamMemberAddress;

/// <summary>
/// Validador para atualização de TeamMemberAddress
/// </summary>
public class UpdateTeamMemberAddressValidator : AbstractValidator<TeamMemberAddressEntity>
{
    private readonly ILocalizationService _localization;

    public UpdateTeamMemberAddressValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberAddress.IdRequired"));

        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Domain.TeamMemberAddress.StreetRequired"))
            .MaximumLength(200)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberAddress.StreetMaxLength"));

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Domain.TeamMemberAddress.CityRequired"))
            .MaximumLength(100)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberAddress.CityMaxLength"));

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Domain.TeamMemberAddress.PostalCodeRequired"))
            .MaximumLength(20)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberAddress.PostalCodeMaxLength"));

        RuleFor(x => x.District)
            .MaximumLength(100)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberAddress.DistrictMaxLength"));

        RuleFor(x => x.CountryCode)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Domain.TeamMemberAddress.CountryCodeRequired"))
            .Length(2)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberAddress.CountryCodeLength"));

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberAddress.ModifiedByRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(_localization.GetMessage("Domain.TeamMemberAddress.CannotUpdateDeleted"));
    }
}
