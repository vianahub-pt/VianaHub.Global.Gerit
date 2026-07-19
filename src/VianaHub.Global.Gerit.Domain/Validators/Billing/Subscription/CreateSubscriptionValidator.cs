using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Billing.Subscription;

public class CreateSubscriptionValidator : AbstractValidator<SubscriptionEntity>
{
    public CreateSubscriptionValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Subscription.TenantIdRequired"));

        RuleFor(x => x.SubscriptionPlanId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Subscription.SubscriptionPlanIdRequired"));

        RuleFor(x => x.StatusDefinitionId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Subscription.StatusDefinitionIdRequired"));

        RuleFor(x => x.StatusDomainId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Subscription.StatusDomainIdRequired"));

        RuleFor(x => x.AgreedAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage(localization.GetMessage("Domain.Subscription.AgreedAmountNonNegative"));

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

        RuleFor(x => x.BillingInterval)
            .MaximumLength(20)
            .WithMessage(localization.GetMessage("Domain.Subscription.BillingIntervalMaxLength", 20))
            .When(x => !string.IsNullOrEmpty(x.BillingInterval));

        RuleFor(x => x.CurrencyCode)
            .MaximumLength(3)
            .WithMessage(localization.GetMessage("Domain.Subscription.CurrencyCodeLength", 3))
            .When(x => !string.IsNullOrEmpty(x.CurrencyCode));
    }
}
