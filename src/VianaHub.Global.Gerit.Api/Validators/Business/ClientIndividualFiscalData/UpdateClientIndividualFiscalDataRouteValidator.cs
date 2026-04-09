using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientIndividualFiscalData;

namespace VianaHub.Global.Gerit.Api.Validators.Business.ClientIndividualFiscalData;

public class UpdateClientIndividualFiscalDataRouteValidator : AbstractValidator<UpdateClientIndividualFiscalDataRequest>
{
    public UpdateClientIndividualFiscalDataRouteValidator()
    {
        RuleFor(x => x.TaxNumber)
            .NotEmpty().WithMessage("TaxNumber_Required")
            .MaximumLength(20).WithMessage("TaxNumber_MaxLength");
    }
}
