using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Intervention;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.Intervention;

/// <summary>
/// Validador para CreateInterventionRequest
/// </summary>
public class CreateInterventionRouteValidator : AbstractValidator<CreateInterventionRequest>
{
    public CreateInterventionRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.ClientId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Api.Validator.Intervention.Create.ClientId"));

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.Intervention.Create.Title"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Api.Validator.Intervention.Create.Title.MaximumLength", 200));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.Intervention.Create.Description"))
            .MaximumLength(2000)
            .WithMessage(localization.GetMessage("Api.Validator.Intervention.Create.Description.MaximumLength", 2000));

        RuleFor(x => x.StartDateTime)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.Intervention.Create.StartDateTime"));

        RuleFor(x => x.EstimatedValue)
            .GreaterThanOrEqualTo(0)
            .WithMessage(localization.GetMessage("Api.Validator.Intervention.Create.EstimatedValue.GreaterThanOrEqual"));
    }
}
