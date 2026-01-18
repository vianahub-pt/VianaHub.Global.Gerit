using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Subscription;

public class CreateSubscriptionValidator : AbstractValidator<SubscriptionEntity>
{
    public CreateSubscriptionValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Subscription.TenantIdRequired"));

        RuleFor(x => x.PlanId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Subscription.PlanIdRequired"));

        RuleFor(x => x.CurrentPeriodStart)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Subscription.CurrentPeriodStartRequired"));

        RuleFor(x => x.CurrentPeriodEnd)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Subscription.CurrentPeriodEndRequired"))
            .GreaterThan(x => x.CurrentPeriodStart)
            .WithMessage(localization.GetMessage("Domain.Subscription.CurrentPeriodEndMustBeAfterStart"));

        RuleFor(x => x.TrialEnd)
            .GreaterThan(x => x.TrialStart)
            .When(x => x.TrialStart.HasValue && x.TrialEnd.HasValue)
            .WithMessage(localization.GetMessage("Domain.Subscription.TrialEndMustBeAfterStart"));

        RuleFor(x => x.StripeCustomerId)
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.Subscription.StripeCustomerIdMaxLength", 100))
            .When(x => !string.IsNullOrEmpty(x.StripeCustomerId));
    }
}
