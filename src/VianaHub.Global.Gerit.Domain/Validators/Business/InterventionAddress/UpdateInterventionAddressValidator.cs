using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.InterventionAddress;

public class UpdateInterventionAddressValidator : AbstractValidator<InterventionAddressEntity>
{
    public UpdateInterventionAddressValidator(ILocalizationService localization)
    {
        RuleFor(x => x.AddressTypeId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Update.AddressTypeId"));

        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Update.Street"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Update.Street.MaximumLength", 200));

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Update.City"))
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Update.City.MaximumLength", 100));

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Update.PostalCode"))
            .MaximumLength(20)
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Update.PostalCode.MaximumLength", 20));

        RuleFor(x => x.District)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.District))
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Update.District.MaximumLength", 100));

        RuleFor(x => x.Neighborhood)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Neighborhood))
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Update.Neighborhood.MaximumLength", 100));

        RuleFor(x => x.StreetNumber)
            .MaximumLength(20)
            .When(x => !string.IsNullOrEmpty(x.StreetNumber))
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Update.StreetNumber.MaximumLength", 20));

        RuleFor(x => x.Complement)
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.Complement))
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Update.Complement.MaximumLength", 200));

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Update.Notes.MaximumLength", 500));

        RuleFor(x => x.CountryCode)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Update.CountryCode"))
            .Length(2)
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Update.CountryCode.Length"));
    }
}
