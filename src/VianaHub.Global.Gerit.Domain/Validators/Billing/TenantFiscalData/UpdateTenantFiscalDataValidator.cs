using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Billing.TenantFiscalData;

/// <summary>
/// Validador para atualização de TenantFiscalData
/// </summary>
public class UpdateTenantFiscalDataValidator : AbstractValidator<TenantFiscalDataEntity>
{
    public UpdateTenantFiscalDataValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.TenantFiscalData.IdRequired"));

        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.TenantFiscalData.TenantIdRequired"));

        RuleFor(x => x.TaxNumber)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.TenantFiscalData.TaxNumberRequired"))
            .MaximumLength(20)
            .WithMessage(localization.GetMessage("Domain.TenantFiscalData.TaxNumberMaxLength", 20));

        RuleFor(x => x.VatNumber)
            .MaximumLength(20)
            .WithMessage(localization.GetMessage("Domain.TenantFiscalData.VatNumberMaxLength", 20))
            .When(x => !string.IsNullOrWhiteSpace(x.VatNumber));

        RuleFor(x => x.FiscalCountry)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.TenantFiscalData.FiscalCountryRequired"))
            .Length(2)
            .WithMessage(localization.GetMessage("Domain.TenantFiscalData.FiscalCountryLength"));

        RuleFor(x => x.IBAN)
            .MaximumLength(34)
            .WithMessage(localization.GetMessage("Domain.TenantFiscalData.IBANMaxLength", 34))
            .When(x => !string.IsNullOrWhiteSpace(x.IBAN));

        RuleFor(x => x.FiscalEmail)
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Domain.TenantFiscalData.FiscalEmailMaxLength", 255))
            .EmailAddress()
            .WithMessage(localization.GetMessage("Domain.TenantFiscalData.FiscalEmailInvalid"))
            .When(x => !string.IsNullOrWhiteSpace(x.FiscalEmail));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.TenantFiscalData.CannotUpdateDeleted"));
    }
}
