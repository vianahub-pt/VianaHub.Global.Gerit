using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Billing.Plan;

public class CreatePlanValidator : AbstractValidator<SubscriptionPlanEntity>
{
    public CreatePlanValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Plan.CodeRequired"))
            .MaximumLength(50)
            .WithMessage(localization.GetMessage("Domain.Plan.CodeMaxLength", 50));

        RuleFor(x => x.Currency)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Plan.CurrencyRequired"))
            .MaximumLength(3)
            .WithMessage(localization.GetMessage("Domain.Plan.CurrencyMaxLength", 3));

        RuleFor(x => x.MaxUsers)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Plan.MaxUsersRequired"));

        RuleFor(x => x.MaxPhotosPerVisit)
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
