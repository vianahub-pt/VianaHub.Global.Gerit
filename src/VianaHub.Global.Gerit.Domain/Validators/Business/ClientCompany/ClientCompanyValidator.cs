using FluentValidation;
using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientCompany;

public class ClientCompanyValidator : AbstractValidator<ClientCompanyEntity>, IEntityDomainValidator<ClientCompanyEntity>
{
    public ClientCompanyValidator()
    {
        RuleFor(x => x.LegalName)
            .NotEmpty().WithMessage("LegalName_Required")
            .MaximumLength(200).WithMessage("LegalName_MaxLength");

        RuleFor(x => x.TradeName)
            .MaximumLength(200).WithMessage("TradeName_MaxLength");

        RuleFor(x => x.Site)
            .MaximumLength(500).WithMessage("Site_MaxLength");

        RuleFor(x => x.CompanyRegistration)
            .MaximumLength(50).WithMessage("CompanyRegistration_MaxLength");

        RuleFor(x => x.CAE)
            .MaximumLength(10).WithMessage("CAE_MaxLength");

        RuleFor(x => x.LegalRepresentative)
            .MaximumLength(150).WithMessage("LegalRepresentative_MaxLength");
    }

    public async Task<ValidationResult> ValidateForCreateAsync(ClientCompanyEntity entity)
        => await new CreateClientCompanyValidator().ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForUpdateAsync(ClientCompanyEntity entity)
        => await new UpdateClientCompanyValidator().ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForActivateAsync(ClientCompanyEntity entity)
        => await new ActivateClientCompanyValidator().ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForDeactivateAsync(ClientCompanyEntity entity)
        => await new DeactivateClientCompanyValidator().ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForDeleteAsync(ClientCompanyEntity entity)
        => await new DeleteClientCompanyValidator().ValidateAsync(entity);

    public Task<ValidationResult> ValidateForRevokeAsync(ClientCompanyEntity entity)
        => ValidateForDeleteAsync(entity);
}
