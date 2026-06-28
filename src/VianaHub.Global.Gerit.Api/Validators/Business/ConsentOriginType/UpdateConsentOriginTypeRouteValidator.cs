using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ConsentOriginType;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.ConsentOriginType;

public class UpdateConsentOriginTypeRouteValidator : AbstractValidator<UpdateConsentOriginTypeRequest>
{
    public UpdateConsentOriginTypeRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.ConsentOriginType.Update.Code"))
            .MaximumLength(50).WithMessage(localization.GetMessage("Api.Validator.ConsentOriginType.Update.Code.MaximumLength", 50));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.ConsentOriginType.Update.Name"))
            .MaximumLength(100).WithMessage(localization.GetMessage("Api.Validator.ConsentOriginType.Update.Name.MaximumLength", 100));

        RuleFor(x => x.Description)
            .MaximumLength(300).WithMessage(localization.GetMessage("Api.Validator.ConsentOriginType.Update.Description.MaximumLength", 300));
    }
}
