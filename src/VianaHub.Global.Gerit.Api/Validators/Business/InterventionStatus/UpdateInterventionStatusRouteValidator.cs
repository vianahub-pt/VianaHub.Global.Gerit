using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionStatus;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Business.InterventionStatus;

/// <summary>
/// Validador para UpdateInterventionStatusRequest
/// </summary>
public class UpdateInterventionStatusRouteValidator : AbstractValidator<UpdateInterventionStatusRequest>
{
    public UpdateInterventionStatusRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.InterventionStatus.Update.Name"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Api.Validator.InterventionStatus.Update.Name.MaximumLength", 200));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.InterventionStatus.Update.Description"))
            .MaximumLength(500)
            .WithMessage(localization.GetMessage("Api.Validator.InterventionStatus.Update.Description.MaximumLength", 500));
    }
}
