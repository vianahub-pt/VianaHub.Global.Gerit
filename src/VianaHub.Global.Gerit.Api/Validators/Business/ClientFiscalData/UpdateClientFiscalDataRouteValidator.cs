using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientFiscalData;

namespace VianaHub.Global.Gerit.Api.Validators.Business.ClientFiscalData;

public class UpdateClientFiscalDataRouteValidator : AbstractValidator<UpdateClientFiscalDataRequest>
{
    public UpdateClientFiscalDataRouteValidator()
    {
        RuleFor(x => x.TaxNumber)
            .NotEmpty().WithMessage("TaxNumber_Required")
            .MaximumLength(20).WithMessage("TaxNumber_MaxLength");
    }
}
