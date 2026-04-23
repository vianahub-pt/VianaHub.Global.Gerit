using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.EmployeeAddress;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.EmployeeAddress;

public class UpdateEmployeeAddressRouteValidator : AbstractValidator<UpdateEmployeeAddressRequest>
{
    private readonly ILocalizationService _localization;

    public UpdateEmployeeAddressRouteValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Api.Validator.EmployeeAddress.Update.Street"))
            .MaximumLength(200)
            .WithMessage(_localization.GetMessage("Api.Validator.EmployeeAddress.Update.Street.MaximumLength", 200));

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Api.Validator.EmployeeAddress.Update.City"))
            .MaximumLength(100)
            .WithMessage(_localization.GetMessage("Api.Validator.EmployeeAddress.Update.City.MaximumLength", 100));

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Api.Validator.EmployeeAddress.Update.PostalCode"))
            .MaximumLength(20)
            .WithMessage(_localization.GetMessage("Api.Validator.EmployeeAddress.Update.PostalCode.MaximumLength", 20));

        RuleFor(x => x.District)
            .MaximumLength(100)
            .WithMessage(_localization.GetMessage("Api.Validator.EmployeeAddress.Update.District.MaximumLength", 100))
            .When(x => !string.IsNullOrEmpty(x.District));

        RuleFor(x => x.CountryCode)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Api.Validator.EmployeeAddress.Update.CountryCode"))
            .Length(2)
            .WithMessage(_localization.GetMessage("Api.Validator.EmployeeAddress.Update.CountryCode.Length"));
    }
}
