using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientType;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.ClientType;

public class UpdateClientTypeRouteValidator : AbstractValidator<UpdateClientTypeRequest>
{
    public UpdateClientTypeRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.ClientType.Update.Name"))
            .MaximumLength(200).WithMessage(localization.GetMessage("Api.Validator.ClientType.Update.Name.MaximumLength", 200));

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.ClientType.Update.Description"))
            .MaximumLength(500).WithMessage(localization.GetMessage("Api.Validator.ClientType.Update.Description.MaximumLength", 500));
    }
}
