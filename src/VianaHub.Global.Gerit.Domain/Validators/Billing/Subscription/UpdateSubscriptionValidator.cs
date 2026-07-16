using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Billing.Subscription;

public class UpdateSubscriptionValidator : AbstractValidator<SubscriptionEntity>
{
    public UpdateSubscriptionValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Subscription.InvalidId"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.Subscription.CannotUpdateDeleted"));

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

        RuleFor(x => x.CurrentPeriodEnd)
            .GreaterThan(x => x.CurrentPeriodStart)
            .WithMessage(localization.GetMessage("Domain.Subscription.CurrentPeriodEndMustBeAfterStart"));
    }
}
