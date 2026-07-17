using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Billing.TenantFiscalData;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Billing.TenantFiscalData;

/// <summary>
/// Validador de rota para criação de TenantFiscalData
/// </summary>
public class CreateTenantFiscalDataRouteValidator : AbstractValidator<CreateTenantFiscalDataRequest>
{
    public CreateTenantFiscalDataRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TaxNumber)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.TenantFiscalData.Create.TaxNumber"))
            .MaximumLength(20)
            .WithMessage(localization.GetMessage("Api.Validator.TenantFiscalData.Create.TaxNumber.MaximumLength", 20));

        RuleFor(x => x.FiscalCountry)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.TenantFiscalData.Create.FiscalCountry"))
            .Length(2)
            .WithMessage(localization.GetMessage("Api.Validator.TenantFiscalData.Create.FiscalCountry.Length"));

        RuleFor(x => x.VatNumber)
            .MaximumLength(20)
            .WithMessage(localization.GetMessage("Api.Validator.TenantFiscalData.Create.VatNumber.MaximumLength", 20))
            .When(x => !string.IsNullOrWhiteSpace(x.VatNumber));

        RuleFor(x => x.IBAN)
            .MaximumLength(34)
            .WithMessage(localization.GetMessage("Api.Validator.TenantFiscalData.Create.IBAN.MaximumLength", 34))
            .When(x => !string.IsNullOrWhiteSpace(x.IBAN));

        RuleFor(x => x.FiscalEmail)
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Api.Validator.TenantFiscalData.Create.FiscalEmail.MaximumLength", 255))
            .EmailAddress()
            .WithMessage(localization.GetMessage("Api.Validator.TenantFiscalData.Create.FiscalEmail.Invalid"))
            .When(x => !string.IsNullOrWhiteSpace(x.FiscalEmail));
    }
}
