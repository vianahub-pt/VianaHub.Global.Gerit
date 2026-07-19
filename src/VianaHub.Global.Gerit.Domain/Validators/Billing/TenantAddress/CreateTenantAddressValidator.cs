using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Billing.TenantAddress;

/// <summary>
/// Validador para criação de TenantAddress
/// </summary>
public class CreateTenantAddressValidator : AbstractValidator<TenantAddressesEntity>
{
    public CreateTenantAddressValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.TenantAddress.TenantIdRequired"));

        RuleFor(x => x.AddressTypeId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.TenantAddress.AddressTypeIdRequired"));

        RuleFor(x => x.CountryCode)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.TenantAddress.CountryCodeRequired"))
            .Length(2)
            .WithMessage(localization.GetMessage("Domain.TenantAddress.CountryCodeLength"));

        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.TenantAddress.StreetRequired"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Domain.TenantAddress.StreetMaxLength", 200));

        RuleFor(x => x.Neighborhood)
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.TenantAddress.NeighborhoodMaxLength", 100))
            .When(x => !string.IsNullOrWhiteSpace(x.Neighborhood));

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.TenantAddress.CityRequired"))
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.TenantAddress.CityMaxLength", 100));

        RuleFor(x => x.District)
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.TenantAddress.DistrictMaxLength", 100))
            .When(x => !string.IsNullOrWhiteSpace(x.District));

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.TenantAddress.PostalCodeRequired"))
            .MaximumLength(20)
            .WithMessage(localization.GetMessage("Domain.TenantAddress.PostalCodeMaxLength", 20));

        RuleFor(x => x.StreetNumber)
            .MaximumLength(20)
            .WithMessage(localization.GetMessage("Domain.TenantAddress.StreetNumberMaxLength", 20))
            .When(x => !string.IsNullOrWhiteSpace(x.StreetNumber));

        RuleFor(x => x.Complement)
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Domain.TenantAddress.ComplementMaxLength", 200))
            .When(x => !string.IsNullOrWhiteSpace(x.Complement));

        RuleFor(x => x.Note)
            .MaximumLength(500)
            .WithMessage(localization.GetMessage("Domain.TenantAddress.NoteMaxLength", 500))
            .When(x => !string.IsNullOrWhiteSpace(x.Note));

        // CK_TenantAddresses_Coordinates: Latitude and Longitude must both be present or both null
        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90m, 90m)
            .WithMessage(localization.GetMessage("Domain.TenantAddress.LatitudeRange"))
            .When(x => x.Latitude.HasValue);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180m, 180m)
            .WithMessage(localization.GetMessage("Domain.TenantAddress.LongitudeRange"))
            .When(x => x.Longitude.HasValue);

        RuleFor(x => x)
            .Must(x => (x.Latitude.HasValue && x.Longitude.HasValue) || (!x.Latitude.HasValue && !x.Longitude.HasValue))
            .WithMessage(localization.GetMessage("Domain.TenantAddress.CoordinatesMustBeBothPresentOrBothNull"));

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.TenantAddress.CreatedByRequired"));
    }
}
