using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Billing.TenantAddress;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Billing.TenantAddress;

/// <summary>
/// Validador de rota para atualização de TenantAddress
/// </summary>
public class UpdateTenantAddressRouteValidator : AbstractValidator<UpdateTenantAddressRequest>
{
    public UpdateTenantAddressRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.AddressTypeId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Api.Validator.TenantAddress.Update.AddressTypeId"));

        RuleFor(x => x.CountryCode)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.TenantAddress.Update.CountryCode"))
            .Length(2)
            .WithMessage(localization.GetMessage("Api.Validator.TenantAddress.Update.CountryCode.Length"));

        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.TenantAddress.Update.Street"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Api.Validator.TenantAddress.Update.Street.MaximumLength", 200));

        RuleFor(x => x.Neighborhood)
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Api.Validator.TenantAddress.Update.Neighborhood.MaximumLength", 100))
            .When(x => !string.IsNullOrWhiteSpace(x.Neighborhood));

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.TenantAddress.Update.City"))
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Api.Validator.TenantAddress.Update.City.MaximumLength", 100));

        RuleFor(x => x.District)
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Api.Validator.TenantAddress.Update.District.MaximumLength", 100))
            .When(x => !string.IsNullOrWhiteSpace(x.District));

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.TenantAddress.Update.PostalCode"))
            .MaximumLength(20)
            .WithMessage(localization.GetMessage("Api.Validator.TenantAddress.Update.PostalCode.MaximumLength", 20));

        RuleFor(x => x.StreetNumber)
            .MaximumLength(20)
            .WithMessage(localization.GetMessage("Api.Validator.TenantAddress.Update.StreetNumber.MaximumLength", 20))
            .When(x => !string.IsNullOrWhiteSpace(x.StreetNumber));

        RuleFor(x => x.Complement)
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Api.Validator.TenantAddress.Update.Complement.MaximumLength", 100))
            .When(x => !string.IsNullOrWhiteSpace(x.Complement));

        RuleFor(x => x.Note)
            .MaximumLength(500)
            .WithMessage(localization.GetMessage("Api.Validator.TenantAddress.Update.Note.MaximumLength", 500))
            .When(x => !string.IsNullOrWhiteSpace(x.Note));

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90m, 90m)
            .WithMessage(localization.GetMessage("Api.Validator.TenantAddress.Update.Latitude.Range"))
            .When(x => x.Latitude.HasValue);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180m, 180m)
            .WithMessage(localization.GetMessage("Api.Validator.TenantAddress.Update.Longitude.Range"))
            .When(x => x.Longitude.HasValue);

        RuleFor(x => x)
            .Must(x => (x.Latitude.HasValue && x.Longitude.HasValue) || (!x.Latitude.HasValue && !x.Longitude.HasValue))
            .WithMessage(localization.GetMessage("Api.Validator.TenantAddress.Update.Coordinates.BothOrNone"));
    }
}
