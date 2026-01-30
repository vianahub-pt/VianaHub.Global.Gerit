using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Identity;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Identity.Action;

/// <summary>
/// Validador completo para ActionEntity
/// </summary>
public class ActionValidator : BaseEntityValidator<ActionEntity>
{
    public ActionValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(ActionEntity entity)
    {
        var validator = new CreateActionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(ActionEntity entity)
    {
        var validator = new UpdateActionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(ActionEntity entity)
    {
        var validator = new ActivateActionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(ActionEntity entity)
    {
        var validator = new DeactivateActionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(ActionEntity entity)
    {
        var validator = new DeleteActionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(ActionEntity entity)
    {
        // TODO: Implement domain rules for revoke. Default: allow (no validation errors).
        return Task.FromResult(new ValidationResult());
    }
}
