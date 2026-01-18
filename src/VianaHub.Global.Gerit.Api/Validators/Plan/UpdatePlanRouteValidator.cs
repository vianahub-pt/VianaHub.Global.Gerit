using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Plan;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Plan;

public class UpdatePlanRouteValidator : AbstractValidator<UpdatePlanRequest>
{
    private readonly ILocalizationService _localization;

    public UpdatePlanRouteValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage(_localization.GetMessage("Api.Validator.Plan.Update.Name.MaximumLength", 100));

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage(_localization.GetMessage("Api.Validator.Plan.Update.Description.MaximumLength", 500));

        RuleFor(x => x.Currency)
            .MaximumLength(3).WithMessage(_localization.GetMessage("Api.Validator.Plan.Update.Currency.MaximumLength", 3));

        When(x => x.MaxUsers > 0, () =>
        {
            RuleFor(x => x.MaxUsers)
                .GreaterThan(0).WithMessage(_localization.GetMessage("Api.Validator.Plan.Update.MaxUsers"));
        });

        When(x => x.MaxPhotosPerInterventions > 0, () =>
        {
            RuleFor(x => x.MaxPhotosPerInterventions)
                .GreaterThan(0).WithMessage(_localization.GetMessage("Api.Validator.Plan.Update.MaxPhotosPerInterventions"));
        });

        When(x => x.PricePerHour.HasValue, () =>
        {
            RuleFor(x => x.PricePerHour)
                .GreaterThanOrEqualTo(0).WithMessage(_localization.GetMessage("Api.Validator.Plan.Update.PricePerHour"));
        });

        When(x => x.PricePerDay.HasValue, () =>
        {
            RuleFor(x => x.PricePerDay)
                .GreaterThanOrEqualTo(0).WithMessage(_localization.GetMessage("Api.Validator.Plan.Update.PricePerDay"));
        });

        When(x => x.PricePerMonth.HasValue, () =>
        {
            RuleFor(x => x.PricePerMonth)
                .GreaterThanOrEqualTo(0).WithMessage(_localization.GetMessage("Api.Validator.Plan.Update.PricePerMonth"));
        });

        When(x => x.PricePerYear.HasValue, () =>
        {
            RuleFor(x => x.PricePerYear)
                .GreaterThanOrEqualTo(0).WithMessage(_localization.GetMessage("Api.Validator.Plan.Update.PricePerYear"));
        });
    }
}
