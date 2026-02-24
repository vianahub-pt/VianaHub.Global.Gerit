using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Intervention;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.Intervention;

/// <summary>
/// Validador para UpdateInterventionRequest
/// </summary>
public class UpdateInterventionRouteValidator : AbstractValidator<UpdateInterventionRequest>
{
    public UpdateInterventionRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.ClientId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Api.Validator.Intervention.Update.ClientId"));

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.Intervention.Update.Title"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Api.Validator.Intervention.Update.Title.MaximumLength", 200));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.Intervention.Update.Description"))
            .MaximumLength(2000)
            .WithMessage(localization.GetMessage("Api.Validator.Intervention.Update.Description.MaximumLength", 2000));

        RuleFor(x => x.StartDateTime)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.Intervention.Update.StartDateTime"));

        RuleFor(x => x.EstimatedValue)
            .GreaterThanOrEqualTo(0)
            .WithMessage(localization.GetMessage("Api.Validator.Intervention.Update.EstimatedValue.GreaterThanOrEqual"));

        RuleFor(x => x.EndDateTime)
            .GreaterThan(x => x.StartDateTime)
            .When(x => x.EndDateTime.HasValue)
            .WithMessage(localization.GetMessage("Api.Validator.Intervention.Update.EndDateTime.GreaterThanStart"));

        RuleFor(x => x.RealValue)
            .GreaterThanOrEqualTo(0)
            .When(x => x.RealValue.HasValue)
            .WithMessage(localization.GetMessage("Api.Validator.Intervention.Update.RealValue.GreaterThanOrEqual"));
    }
}
