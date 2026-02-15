using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.StatusType;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Business.StatusType;

public class UpdateStatusTypeRouteValidator : AbstractValidator<UpdateStatusTypeRequest>
{
    public UpdateStatusTypeRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.StatusType.Update.Name"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Api.Validator.StatusType.Update.Name.MaximumLength", 150));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.StatusType.Update.Description"))
            .MaximumLength(500)
            .WithMessage(localization.GetMessage("Api.Validator.StatusType.Update.Description.MaximumLength", 500));
    }
}
