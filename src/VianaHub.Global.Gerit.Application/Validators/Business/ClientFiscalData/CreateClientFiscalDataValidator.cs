using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientFiscalData;

namespace VianaHub.Global.Gerit.Application.Validators.Business.ClientFiscalData;

public class CreateClientFiscalDataValidator : AbstractValidator<CreateClientFiscalDataRequest>
{
    public CreateClientFiscalDataValidator()
    {
        RuleFor(x => x.TaxNumber)
            .NotEmpty().WithMessage("TaxNumber_Required")
            .MaximumLength(20).WithMessage("TaxNumber_MaxLength");

        RuleFor(x => x.VatNumber)
            .MaximumLength(20).WithMessage("VatNumber_MaxLength");

        RuleFor(x => x.FiscalCountry)
            .NotEmpty().WithMessage("FiscalCountry_Required")
            .Length(2).WithMessage("FiscalCountry_Length");

        RuleFor(x => x.IBAN)
            .MaximumLength(34).WithMessage("IBAN_MaxLength");

        RuleFor(x => x.FiscalEmail)
            .MaximumLength(255).WithMessage("FiscalEmail_MaxLength")
            .EmailAddress().WithMessage("FiscalEmail_Invalid")
            .When(x => !string.IsNullOrEmpty(x.FiscalEmail));
    }
}
