using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ConsentType;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.ConsentType;

public class CreateConsentTypeRouteValidator : AbstractValidator<CreateConsentTypeRequest>
{
    public CreateConsentTypeRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.ConsentType.Create.Name"))
            .MaximumLength(200).WithMessage(localization.GetMessage("Api.Validator.ConsentType.Create.Name.MaximumLength", 200));

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.ConsentType.Create.Description"))
            .MaximumLength(500).WithMessage(localization.GetMessage("Api.Validator.ConsentType.Create.Description.MaximumLength", 500));
    }
}
