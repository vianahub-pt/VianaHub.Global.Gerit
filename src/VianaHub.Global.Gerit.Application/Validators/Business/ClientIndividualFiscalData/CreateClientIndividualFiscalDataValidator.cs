using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientIndividualFiscalData;

namespace VianaHub.Global.Gerit.Application.Validators.Business.ClientIndividualFiscalData;

public class CreateClientIndividualFiscalDataValidator : AbstractValidator<CreateClientIndividualFiscalDataRequest>
{
    public CreateClientIndividualFiscalDataValidator()
    {
        RuleFor(x => x.ClientIndividualId)
            .GreaterThan(0).WithMessage("ClientIndividualId_Required");

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
