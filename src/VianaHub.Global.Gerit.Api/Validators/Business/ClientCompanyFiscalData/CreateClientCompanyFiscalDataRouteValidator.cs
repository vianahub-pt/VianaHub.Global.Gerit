using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientCompanyFiscalData;

namespace VianaHub.Global.Gerit.Api.Validators.Business.ClientCompanyFiscalData;

public class CreateClientCompanyFiscalDataRouteValidator : AbstractValidator<CreateClientCompanyFiscalDataRequest>
{
    public CreateClientCompanyFiscalDataRouteValidator()
    {
        RuleFor(x => x.ClientCompanyId)
            .GreaterThan(0).WithMessage("ClientCompanyId_Required");

        RuleFor(x => x.TaxNumber)
            .NotEmpty().WithMessage("TaxNumber_Required")
            .MaximumLength(20).WithMessage("TaxNumber_MaxLength");
    }
}
