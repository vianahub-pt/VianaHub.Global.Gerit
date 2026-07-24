using FluentValidation;
using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.EmployeeFiscalData;

public class EmployeeFiscalDataValidator : AbstractValidator<EmployeeFiscalDataEntity>, IEntityDomainValidator<EmployeeFiscalDataEntity>
{
    public EmployeeFiscalDataValidator()
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

    public async Task<ValidationResult> ValidateForCreateAsync(EmployeeFiscalDataEntity entity)
        => await new CreateEmployeeFiscalDataValidator().ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForUpdateAsync(EmployeeFiscalDataEntity entity)
        => await new UpdateEmployeeFiscalDataValidator().ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForActivateAsync(EmployeeFiscalDataEntity entity)
        => await new ActivateEmployeeFiscalDataValidator().ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForDeactivateAsync(EmployeeFiscalDataEntity entity)
        => await new DeactivateEmployeeFiscalDataValidator().ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForDeleteAsync(EmployeeFiscalDataEntity entity)
        => await new DeleteEmployeeFiscalDataValidator().ValidateAsync(entity);

    public Task<ValidationResult> ValidateForRevokeAsync(EmployeeFiscalDataEntity entity)
        => ValidateForDeleteAsync(entity);
}
