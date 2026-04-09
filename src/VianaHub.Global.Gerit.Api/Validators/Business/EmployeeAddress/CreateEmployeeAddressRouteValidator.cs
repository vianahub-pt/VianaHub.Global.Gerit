using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.EmployeeAddress;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.EmployeeAddress;

public class CreateEmployeeAddressRouteValidator : AbstractValidator<CreateEmployeeAddressRequest>
{
    private readonly ILocalizationService _localization;

    public CreateEmployeeAddressRouteValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Api.Validator.EmployeeAddress.Create.EmployeeId"));

        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Api.Validator.EmployeeAddress.Create.Street"))
            .MaximumLength(200)
            .WithMessage(_localization.GetMessage("Api.Validator.EmployeeAddress.Create.Street.MaximumLength", 200));

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Api.Validator.EmployeeAddress.Create.City"))
            .MaximumLength(100)
            .WithMessage(_localization.GetMessage("Api.Validator.EmployeeAddress.Create.City.MaximumLength", 100));

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Api.Validator.EmployeeAddress.Create.PostalCode"))
            .MaximumLength(20)
            .WithMessage(_localization.GetMessage("Api.Validator.EmployeeAddress.Create.PostalCode.MaximumLength", 20));

        RuleFor(x => x.District)
            .MaximumLength(100)
            .WithMessage(_localization.GetMessage("Api.Validator.EmployeeAddress.Create.District.MaximumLength", 100))
            .When(x => !string.IsNullOrEmpty(x.District));

        RuleFor(x => x.CountryCode)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Api.Validator.EmployeeAddress.Create.CountryCode"))
            .Length(2)
            .WithMessage(_localization.GetMessage("Api.Validator.EmployeeAddress.Create.CountryCode.Length"));
    }
}
