using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientCompany;

namespace VianaHub.Global.Gerit.Application.Validators.Business.ClientCompany;

public class UpdateClientCompanyValidator : AbstractValidator<UpdateClientCompanyRequest>
{
    public UpdateClientCompanyValidator()
    {
        RuleFor(x => x.LegalName)
            .NotEmpty().WithMessage("LegalName_Required")
            .MaximumLength(200).WithMessage("LegalName_MaxLength");

        RuleFor(x => x.TradeName)
            .MaximumLength(200).WithMessage("TradeName_MaxLength");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(50).WithMessage("PhoneNumber_MaxLength");

        RuleFor(x => x.CellPhoneNumber)
            .MaximumLength(50).WithMessage("CellPhoneNumber_MaxLength");

        RuleFor(x => x.Email)
            .MaximumLength(500).WithMessage("Email_MaxLength")
            .EmailAddress().WithMessage("Email_Invalid")
            .When(x => !string.IsNullOrEmpty(x.Email));

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
