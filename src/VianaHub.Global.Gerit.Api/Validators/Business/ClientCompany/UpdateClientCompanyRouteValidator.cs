using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientCompany;

namespace VianaHub.Global.Gerit.Api.Validators.Business.ClientCompany;

public class UpdateClientCompanyRouteValidator : AbstractValidator<UpdateClientCompanyRequest>
{
    public UpdateClientCompanyRouteValidator()
    {
        RuleFor(x => x.LegalName)
            .NotEmpty().WithMessage("LegalName_Required")
            .MaximumLength(200).WithMessage("LegalName_MaxLength");
    }
}
