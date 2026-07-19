using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Billing.Subscription;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Billing.Subscription;

public class CreateSubscriptionRouteValidator : AbstractValidator<CreateSubscriptionRequest>
{
    private readonly ILocalizationService _localization;

    public CreateSubscriptionRouteValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.TenantId)
            .GreaterThan(0).WithMessage(_localization.GetMessage("Api.Validator.Subscription.Create.TenantId"));

        RuleFor(x => x.SubscriptionPlanId)
            .GreaterThan(0).WithMessage(_localization.GetMessage("Api.Validator.Subscription.Create.SubscriptionPlanId"));

        RuleFor(x => x.StatusDefinitionId)
            .GreaterThan(0).WithMessage(_localization.GetMessage("Api.Validator.Subscription.Create.StatusDefinitionId"));

        RuleFor(x => x.StatusDomainId)
            .GreaterThan(0).WithMessage(_localization.GetMessage("Api.Validator.Subscription.Create.StatusDomainId"));

        RuleFor(x => x.AgreedAmount)
            .GreaterThanOrEqualTo(0).WithMessage(_localization.GetMessage("Api.Validator.Subscription.Create.AgreedAmountNonNegative"));

        RuleFor(x => x.CurrentPeriodStart)
            .NotEmpty().WithMessage(_localization.GetMessage("Api.Validator.Subscription.Create.CurrentPeriodStart"));

        RuleFor(x => x.CurrentPeriodEnd)
            .NotEmpty().WithMessage(_localization.GetMessage("Api.Validator.Subscription.Create.CurrentPeriodEnd"))
            .GreaterThan(x => x.CurrentPeriodStart).WithMessage(_localization.GetMessage("Api.Validator.Subscription.Create.CurrentPeriodEnd.AfterStart"));

        When(x => x.TrialStart.HasValue && x.TrialEnd.HasValue, () =>
        {
            RuleFor(x => x.TrialEnd)
                .GreaterThan(x => x.TrialStart).WithMessage(_localization.GetMessage("Api.Validator.Subscription.Create.TrialEnd.AfterStart"));
        });

        RuleFor(x => x.StripeCustomerId)
            .MaximumLength(100).WithMessage(_localization.GetMessage("Api.Validator.Subscription.Create.StripeCustomerId.MaximumLength", 100));

        RuleFor(x => x.BillingInterval)
            .MaximumLength(20).WithMessage(_localization.GetMessage("Api.Validator.Subscription.Create.BillingInterval.MaximumLength", 20));

        RuleFor(x => x.CurrencyCode)
            .MaximumLength(3).WithMessage(_localization.GetMessage("Api.Validator.Subscription.Create.CurrencyCode.Length", 3));
    }
}
