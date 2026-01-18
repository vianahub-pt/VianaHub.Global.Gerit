using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Subscription;

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

        RuleFor(x => x.PlanId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Subscription.PlanIdRequired"));

        RuleFor(x => x.CurrentPeriodEnd)
            .GreaterThan(x => x.CurrentPeriodStart)
            .WithMessage(localization.GetMessage("Domain.Subscription.CurrentPeriodEndMustBeAfterStart"));
    }
}
