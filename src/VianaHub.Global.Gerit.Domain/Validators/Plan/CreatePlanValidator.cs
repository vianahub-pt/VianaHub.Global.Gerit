using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Plan;

public class CreatePlanValidator : AbstractValidator<PlanEntity>
{
    public CreatePlanValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Plan.NameRequired"))
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.Plan.NameMaxLength", 100));

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage(localization.GetMessage("Domain.Plan.DescriptionMaxLength", 500))
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Currency)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Plan.CurrencyRequired"))
            .MaximumLength(3)
            .WithMessage(localization.GetMessage("Domain.Plan.CurrencyMaxLength", 3));

        RuleFor(x => x.MaxUsers)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Plan.MaxUsersRequired"));

        RuleFor(x => x.MaxPhotosPerInterventions)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Plan.MaxPhotosRequired"));

        RuleFor(x => x.PricePerHour)
            .GreaterThanOrEqualTo(0)
            .WithMessage(localization.GetMessage("Domain.Plan.PricePerHourInvalid"))
            .When(x => x.PricePerHour.HasValue);

        RuleFor(x => x.PricePerDay)
            .GreaterThanOrEqualTo(0)
            .WithMessage(localization.GetMessage("Domain.Plan.PricePerDayInvalid"))
            .When(x => x.PricePerDay.HasValue);

        RuleFor(x => x.PricePerMonth)
            .GreaterThanOrEqualTo(0)
            .WithMessage(localization.GetMessage("Domain.Plan.PricePerMonthInvalid"))
            .When(x => x.PricePerMonth.HasValue);

        RuleFor(x => x.PricePerYear)
            .GreaterThanOrEqualTo(0)
            .WithMessage(localization.GetMessage("Domain.Plan.PricePerYearInvalid"))
            .When(x => x.PricePerYear.HasValue);
    }
}
