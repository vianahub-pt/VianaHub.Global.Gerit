using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientCompanyFiscalData;

namespace VianaHub.Global.Gerit.Api.Validators.Business.ClientCompanyFiscalData;

public class UpdateClientCompanyFiscalDataRouteValidator : AbstractValidator<UpdateClientCompanyFiscalDataRequest>
{
    public UpdateClientCompanyFiscalDataRouteValidator()
    {
        RuleFor(x => x.TaxNumber)
            .NotEmpty().WithMessage("TaxNumber_Required")
            .MaximumLength(20).WithMessage("TaxNumber_MaxLength");
    }
}
