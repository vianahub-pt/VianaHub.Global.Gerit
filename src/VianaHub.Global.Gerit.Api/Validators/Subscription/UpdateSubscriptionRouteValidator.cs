using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Subscription;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Subscription;

public class UpdateSubscriptionRouteValidator : AbstractValidator<UpdateSubscriptionRequest>
{
    private readonly ILocalizationService _localization;

    public UpdateSubscriptionRouteValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.PlanId)
            .GreaterThan(0).WithMessage(_localization.GetMessage("Api.Validator.Subscription.Update.PlanId"));

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
