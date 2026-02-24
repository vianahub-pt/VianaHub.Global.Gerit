using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientAddress;

/// <summary>
/// Validador para criação de ClientAddress
/// </summary>
public class CreateClientAddressValidator : AbstractValidator<ClientAddressEntity>
{
    public CreateClientAddressValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.ClientAddress.TenantIdRequired"));

        RuleFor(x => x.ClientId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.ClientAddress.ClientIdRequired"));

        RuleFor(x => x.AddressTypeId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.ClientAddress.AddressTypeIdRequired"));

        RuleFor(x => x.CountryCode)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.ClientAddress.CountryCodeRequired"))
            .Length(2)
            .WithMessage(localization.GetMessage("Domain.ClientAddress.CountryCodeLength"));

        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.ClientAddress.StreetRequired"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Domain.ClientAddress.StreetMaxLength"));

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.ClientAddress.CityRequired"))
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.ClientAddress.CityMaxLength"));

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.ClientAddress.PostalCodeRequired"))
            .MaximumLength(20)
            .WithMessage(localization.GetMessage("Domain.ClientAddress.PostalCodeMaxLength"));

        RuleFor(x => x.District)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.District))
            .WithMessage(localization.GetMessage("Domain.ClientAddress.DistrictMaxLength"));

        RuleFor(x => x.Neighborhood)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Neighborhood))
            .WithMessage(localization.GetMessage("Domain.ClientAddress.NeighborhoodMaxLength"));

        RuleFor(x => x.StreetNumber)
            .MaximumLength(20)
            .When(x => !string.IsNullOrEmpty(x.StreetNumber))
            .WithMessage(localization.GetMessage("Domain.ClientAddress.StreetNumberMaxLength"));

        RuleFor(x => x.Complement)
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.Complement))
            .WithMessage(localization.GetMessage("Domain.ClientAddress.ComplementMaxLength"));

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage(localization.GetMessage("Domain.ClientAddress.NotesMaxLength"));

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.ClientAddress.CreatedByRequired"));
    }
}
