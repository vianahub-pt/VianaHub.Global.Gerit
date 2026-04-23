using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientCompany;

namespace VianaHub.Global.Gerit.Api.Validators.Business.ClientCompany;

public class CreateClientCompanyRouteValidator : AbstractValidator<CreateClientCompanyRequest>
{
    public CreateClientCompanyRouteValidator()
    {
        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("ClientId_Required");

        RuleFor(x => x.LegalName)
            .NotEmpty().WithMessage("LegalName_Required")
            .MaximumLength(200).WithMessage("LegalName_MaxLength");
    }
}
