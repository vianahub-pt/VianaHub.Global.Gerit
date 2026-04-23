using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.EmployeeAddress;

/// <summary>
/// Validador para cria��o de EmployeeAddress
/// </summary>
public class CreateEmployeeAddressValidator : AbstractValidator<EmployeeAddressEntity>
{
    private readonly ILocalizationService _localization;

    public CreateEmployeeAddressValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.EmployeeAddress.TenantIdRequired"));

        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.EmployeeAddress.EmployeeIdRequired"));

        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Domain.EmployeeAddress.StreetRequired"))
            .MaximumLength(200)
            .WithMessage(_localization.GetMessage("Domain.EmployeeAddress.StreetMaxLength"));

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Domain.EmployeeAddress.CityRequired"))
            .MaximumLength(100)
            .WithMessage(_localization.GetMessage("Domain.EmployeeAddress.CityMaxLength"));

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Domain.EmployeeAddress.PostalCodeRequired"))
            .MaximumLength(20)
            .WithMessage(_localization.GetMessage("Domain.EmployeeAddress.PostalCodeMaxLength"));

        RuleFor(x => x.District)
            .MaximumLength(100)
            .WithMessage(_localization.GetMessage("Domain.EmployeeAddress.DistrictMaxLength"));

        RuleFor(x => x.CountryCode)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Domain.EmployeeAddress.CountryCodeRequired"))
            .Length(2)
            .WithMessage(_localization.GetMessage("Domain.EmployeeAddress.CountryCodeLength"));

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.EmployeeAddress.CreatedByRequired"));
    }
}
