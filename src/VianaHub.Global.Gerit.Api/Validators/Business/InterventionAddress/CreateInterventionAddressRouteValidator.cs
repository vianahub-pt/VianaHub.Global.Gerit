using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionAddress;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Business.InterventionAddress;

/// <summary>
/// Validador de rota para criação de InterventionAddress
/// </summary>
public class CreateInterventionAddressRouteValidator : AbstractValidator<CreateInterventionAddressRequest>
{
    public CreateInterventionAddressRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.InterventionId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Create.InterventionId"));

        RuleFor(x => x.AddressTypeId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Create.AddressTypeId"));

        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Create.Street"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Create.Street.MaximumLength", 200));

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Create.City"))
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Create.City.MaximumLength", 100));

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Create.PostalCode"))
            .MaximumLength(20)
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Create.PostalCode.MaximumLength", 20));

        RuleFor(x => x.District)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.District))
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Create.District.MaximumLength", 100));

        RuleFor(x => x.Neighborhood)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Neighborhood))
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Create.Neighborhood.MaximumLength", 100));

        RuleFor(x => x.StreetNumber)
            .MaximumLength(20)
            .When(x => !string.IsNullOrEmpty(x.StreetNumber))
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Create.StreetNumber.MaximumLength", 20));

        RuleFor(x => x.Complement)
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.Complement))
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Create.Complement.MaximumLength", 200));

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Create.Notes.MaximumLength", 500));

        RuleFor(x => x.CountryCode)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Create.CountryCode"))
            .Length(2)
            .WithMessage(localization.GetMessage("Api.Validator.InterventionAddress.Create.CountryCode.Length"));
    }
}
