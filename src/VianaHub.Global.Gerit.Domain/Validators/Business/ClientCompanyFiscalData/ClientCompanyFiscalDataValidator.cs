using FluentValidation;
using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientCompanyFiscalData;

public class ClientCompanyFiscalDataValidator : AbstractValidator<ClientCompanyFiscalDataEntity>, IEntityDomainValidator<ClientCompanyFiscalDataEntity>
{
    public ClientCompanyFiscalDataValidator()
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

    public async Task<ValidationResult> ValidateForCreateAsync(ClientCompanyFiscalDataEntity entity)
        => await new CreateClientCompanyFiscalDataValidator().ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForUpdateAsync(ClientCompanyFiscalDataEntity entity)
        => await new UpdateClientCompanyFiscalDataValidator().ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForActivateAsync(ClientCompanyFiscalDataEntity entity)
        => await new ActivateClientCompanyFiscalDataValidator().ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForDeactivateAsync(ClientCompanyFiscalDataEntity entity)
        => await new DeactivateClientCompanyFiscalDataValidator().ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForDeleteAsync(ClientCompanyFiscalDataEntity entity)
        => await new DeleteClientCompanyFiscalDataValidator().ValidateAsync(entity);

    public Task<ValidationResult> ValidateForRevokeAsync(ClientCompanyFiscalDataEntity entity)
        => ValidateForDeleteAsync(entity);
}
