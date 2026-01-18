using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Plan;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Plan;

public class CreatePlanRouteValidator : AbstractValidator<CreatePlanRequest>
{
    private readonly ILocalizationService _localization;

    public CreatePlanRouteValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(_localization.GetMessage("Api.Validator.Plan.Create.Name"))
            .MaximumLength(100).WithMessage(_localization.GetMessage("Api.Validator.Plan.Create.Name.MaximumLength", 100));

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage(_localization.GetMessage("Api.Validator.Plan.Create.Description.MaximumLength", 500));

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage(_localization.GetMessage("Api.Validator.Plan.Create.Currency"))
            .MaximumLength(3).WithMessage(_localization.GetMessage("Api.Validator.Plan.Create.Currency.MaximumLength", 3));

        RuleFor(x => x.MaxUsers)
            .GreaterThan(0).WithMessage(_localization.GetMessage("Api.Validator.Plan.Create.MaxUsers"));

        RuleFor(x => x.MaxPhotosPerInterventions)
            .GreaterThan(0).WithMessage(_localization.GetMessage("Api.Validator.Plan.Create.MaxPhotosPerInterventions"));

        When(x => x.PricePerHour.HasValue, () =>
        {
            RuleFor(x => x.PricePerHour)
                .GreaterThanOrEqualTo(0).WithMessage(_localization.GetMessage("Api.Validator.Plan.Create.PricePerHour"));
        });

        When(x => x.PricePerDay.HasValue, () =>
        {
            RuleFor(x => x.PricePerDay)
                .GreaterThanOrEqualTo(0).WithMessage(_localization.GetMessage("Api.Validator.Plan.Create.PricePerDay"));
        });

        When(x => x.PricePerMonth.HasValue, () =>
        {
            RuleFor(x => x.PricePerMonth)
                .GreaterThanOrEqualTo(0).WithMessage(_localization.GetMessage("Api.Validator.Plan.Create.PricePerMonth"));
        });

        When(x => x.PricePerYear.HasValue, () =>
        {
            RuleFor(x => x.PricePerYear)
                .GreaterThanOrEqualTo(0).WithMessage(_localization.GetMessage("Api.Validator.Plan.Create.PricePerYear"));
        });
    }
}
