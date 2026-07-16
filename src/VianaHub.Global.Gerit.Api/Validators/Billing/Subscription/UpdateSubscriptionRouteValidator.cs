using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Billing.Subscription;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Billing.Subscription;

public class UpdateSubscriptionRouteValidator : AbstractValidator<UpdateSubscriptionRequest>
{
    private readonly ILocalizationService _localization;

    public UpdateSubscriptionRouteValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.SubscriptionPlanId)
            .GreaterThan(0).WithMessage(_localization.GetMessage("Api.Validator.Subscription.Update.SubscriptionPlanId"));

        RuleFor(x => x.StatusDefinitionId)
            .GreaterThan(0).WithMessage(_localization.GetMessage("Api.Validator.Subscription.Update.StatusDefinitionId"));

        RuleFor(x => x.StatusDomainId)
            .GreaterThan(0).WithMessage(_localization.GetMessage("Api.Validator.Subscription.Update.StatusDomainId"));

        RuleFor(x => x.AgreedAmount)
            .GreaterThanOrEqualTo(0).WithMessage(_localization.GetMessage("Api.Validator.Subscription.Update.AgreedAmountNonNegative"));

        RuleFor(x => x.CurrentPeriodStart)
            .NotEmpty().WithMessage(_localization.GetMessage("Api.Validator.Subscription.Update.CurrentPeriodStart"));

        RuleFor(x => x.CurrentPeriodEnd)
            .NotEmpty().WithMessage(_localization.GetMessage("Api.Validator.Subscription.Update.CurrentPeriodEnd"))
            .GreaterThan(x => x.CurrentPeriodStart).WithMessage(_localization.GetMessage("Api.Validator.Subscription.Update.CurrentPeriodEnd.AfterStart"));

        When(x => x.TrialStart.HasValue && x.TrialEnd.HasValue, () =>
        {
            RuleFor(x => x.TrialEnd)
                .GreaterThan(x => x.TrialStart).WithMessage(_localization.GetMessage("Api.Validator.Subscription.Update.TrialEnd.AfterStart"));
        });
    }
}
