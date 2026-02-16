using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Status;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Business.Status;

/// <summary>
/// Validador para UpdateStatusRequest
/// </summary>
public class UpdateStatusRouteValidator : AbstractValidator<UpdateStatusRequest>
{
    public UpdateStatusRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.StatusTypeId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Api.Validator.Status.Update.StatusTypeId"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.Status.Update.Name"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Api.Validator.Status.Update.Name.MaximumLength", 200));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.Status.Update.Description"))
            .MaximumLength(500)
            .WithMessage(localization.GetMessage("Api.Validator.Status.Update.Description.MaximumLength", 500));
    }
}
