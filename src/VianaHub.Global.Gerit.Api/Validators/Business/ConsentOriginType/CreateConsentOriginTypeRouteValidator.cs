using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ConsentOriginType;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.ConsentOriginType;

public class CreateConsentOriginTypeRouteValidator : AbstractValidator<CreateConsentOriginTypeRequest>
{
    public CreateConsentOriginTypeRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.ConsentOriginType.Create.Code"))
            .MaximumLength(50).WithMessage(localization.GetMessage("Api.Validator.ConsentOriginType.Create.Code.MaximumLength", 50));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.ConsentOriginType.Create.Name"))
            .MaximumLength(100).WithMessage(localization.GetMessage("Api.Validator.ConsentOriginType.Create.Name.MaximumLength", 100));

        RuleFor(x => x.Description)
            .MaximumLength(300).WithMessage(localization.GetMessage("Api.Validator.ConsentOriginType.Create.Description.MaximumLength", 300));
    }
}
