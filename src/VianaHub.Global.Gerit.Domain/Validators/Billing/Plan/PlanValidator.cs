using FluentValidation;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Billing.Plan;

public class PlanValidator : IEntityDomainValidator<SubscriptionPlanEntity>
{
    private readonly CreatePlanValidator _createValidator;
    private readonly UpdatePlanValidator _updateValidator;
    private readonly ActivatePlanValidator _activateValidator;
    private readonly DeactivatePlanValidator _deactivateValidator;
    private readonly DeletePlanValidator _deleteValidator;

    public PlanValidator(ILocalizationService localization)
    {
        _createValidator = new CreatePlanValidator(localization);
        _updateValidator = new UpdatePlanValidator(localization);
        _activateValidator = new ActivatePlanValidator(localization);
        _deactivateValidator = new DeactivatePlanValidator(localization);
        _deleteValidator = new DeletePlanValidator(localization);
    }

    public async Task<FluentValidation.Results.ValidationResult> ValidateForCreateAsync(SubscriptionPlanEntity entity)
    {
        return await _createValidator.ValidateAsync(entity);
    }

    public async Task<FluentValidation.Results.ValidationResult> ValidateForUpdateAsync(SubscriptionPlanEntity entity)
    {
        return await _updateValidator.ValidateAsync(entity);
    }

    public async Task<FluentValidation.Results.ValidationResult> ValidateForActivateAsync(SubscriptionPlanEntity entity)
    {
        return await _activateValidator.ValidateAsync(entity);
    }

    public async Task<FluentValidation.Results.ValidationResult> ValidateForDeactivateAsync(SubscriptionPlanEntity entity)
    {
        return await _deactivateValidator.ValidateAsync(entity);
    }

    public async Task<FluentValidation.Results.ValidationResult> ValidateForDeleteAsync(SubscriptionPlanEntity entity)
    {
        return await _deleteValidator.ValidateAsync(entity);
    }

    public async Task<FluentValidation.Results.ValidationResult> ValidateForRevokeAsync(SubscriptionPlanEntity entity)
    {
        // Plans n�o t�m opera��o de revoga��o, retorna sempre v�lido
        return await Task.FromResult(new FluentValidation.Results.ValidationResult());
    }
}
