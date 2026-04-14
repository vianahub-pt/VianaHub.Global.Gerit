using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientCompany;

public class ClientCompanyValidator : AbstractValidator<ClientCompanyEntity>
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
}
