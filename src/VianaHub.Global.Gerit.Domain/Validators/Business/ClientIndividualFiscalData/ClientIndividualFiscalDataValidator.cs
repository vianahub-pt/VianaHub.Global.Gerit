using FluentValidation;
using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientIndividualFiscalData;

public class ClientIndividualFiscalDataValidator : AbstractValidator<ClientIndividualFiscalDataEntity>, IEntityDomainValidator<ClientIndividualFiscalDataEntity>
{
    public ClientIndividualFiscalDataValidator()
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

    public async Task<ValidationResult> ValidateForCreateAsync(ClientIndividualFiscalDataEntity entity)
        => await new CreateClientIndividualFiscalDataValidator().ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForUpdateAsync(ClientIndividualFiscalDataEntity entity)
        => await new UpdateClientIndividualFiscalDataValidator().ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForActivateAsync(ClientIndividualFiscalDataEntity entity)
        => await new ActivateClientIndividualFiscalDataValidator().ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForDeactivateAsync(ClientIndividualFiscalDataEntity entity)
        => await new DeactivateClientIndividualFiscalDataValidator().ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForDeleteAsync(ClientIndividualFiscalDataEntity entity)
        => await new DeleteClientIndividualFiscalDataValidator().ValidateAsync(entity);

    public Task<ValidationResult> ValidateForRevokeAsync(ClientIndividualFiscalDataEntity entity)
        => ValidateForDeleteAsync(entity);
}
