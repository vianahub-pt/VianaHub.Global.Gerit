using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Visit;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.Visit;

/// <summary>
/// Validador para CreateVisitRequest
/// </summary>
public class CreateVisitRouteValidator : AbstractValidator<CreateVisitRequest>
{
    public CreateVisitRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.ClientId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Api.Validator.Visit.Create.ClientId"));

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.Visit.Create.Title"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Api.Validator.Visit.Create.Title.MaximumLength", 200));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.Visit.Create.Description"))
            .MaximumLength(2000)
            .WithMessage(localization.GetMessage("Api.Validator.Visit.Create.Description.MaximumLength", 2000));

        RuleFor(x => x.StartDateTime)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.Visit.Create.StartDateTime"));

        RuleFor(x => x.EstimatedValue)
            .GreaterThanOrEqualTo(0)
            .WithMessage(localization.GetMessage("Api.Validator.Visit.Create.EstimatedValue.GreaterThanOrEqual"));
    }
}
