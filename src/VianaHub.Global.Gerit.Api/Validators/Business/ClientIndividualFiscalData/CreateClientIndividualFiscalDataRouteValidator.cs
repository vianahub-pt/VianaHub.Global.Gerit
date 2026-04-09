using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientIndividualFiscalData;

namespace VianaHub.Global.Gerit.Api.Validators.Business.ClientIndividualFiscalData;

public class CreateClientIndividualFiscalDataRouteValidator : AbstractValidator<CreateClientIndividualFiscalDataRequest>
{
    public CreateClientIndividualFiscalDataRouteValidator()
    {
        RuleFor(x => x.ClientIndividualId)
            .GreaterThan(0).WithMessage("ClientIndividualId_Required");

        RuleFor(x => x.TaxNumber)
            .NotEmpty().WithMessage("TaxNumber_Required")
            .MaximumLength(20).WithMessage("TaxNumber_MaxLength");
    }
}
