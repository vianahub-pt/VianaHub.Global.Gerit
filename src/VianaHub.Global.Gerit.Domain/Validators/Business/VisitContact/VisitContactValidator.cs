using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitContact;

/// <summary>
/// Validador completo para VisitContactEntity
/// </summary>
public class VisitContactValidator : BaseEntityValidator<VisitContactEntity>
{
    public VisitContactValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(VisitContactEntity entity)
    {
        var validator = new CreateVisitContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(VisitContactEntity entity)
    {
        var validator = new UpdateVisitContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(VisitContactEntity entity)
    {
        var validator = new ActivateVisitContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(VisitContactEntity entity)
    {
        var validator = new DeactivateVisitContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(VisitContactEntity entity)
    {
        var validator = new DeleteVisitContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(VisitContactEntity entity)
    {
        // Não aplicável para VisitContact
        return Task.FromResult(new ValidationResult());
    }
}
