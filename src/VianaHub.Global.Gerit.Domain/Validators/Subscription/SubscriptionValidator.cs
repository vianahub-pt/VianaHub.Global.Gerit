using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Subscription;

/// <summary>
/// Validador completo para SubscriptionEntity
/// </summary>
public class SubscriptionValidator : BaseEntityValidator<SubscriptionEntity>
{
    public SubscriptionValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(SubscriptionEntity entity)
    {
        var validator = new CreateSubscriptionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(SubscriptionEntity entity)
    {
        var validator = new UpdateSubscriptionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(SubscriptionEntity entity)
    {
        var validator = new ActivateSubscriptionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(SubscriptionEntity entity)
    {
        var validator = new DeactivateSubscriptionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(SubscriptionEntity entity)
    {
        var validator = new DeleteSubscriptionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(SubscriptionEntity entity)
    {
        // Não aplicável para subscriptions
        return Task.FromResult(new ValidationResult());
    }
}
