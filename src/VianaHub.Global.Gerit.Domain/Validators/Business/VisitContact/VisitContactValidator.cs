using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitContact;

/// <summary>
/// Validador completo para VisitContactPersonsEntity
/// </summary>
public class VisitContactValidator : BaseEntityValidator<VisitContactPersonsEntity>
{
    public VisitContactValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(VisitContactPersonsEntity entity)
    {
        var validator = new CreateVisitContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(VisitContactPersonsEntity entity)
    {
        var validator = new UpdateVisitContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(VisitContactPersonsEntity entity)
    {
        var validator = new ActivateVisitContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(VisitContactPersonsEntity entity)
    {
        var validator = new DeactivateVisitContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(VisitContactPersonsEntity entity)
    {
        var validator = new DeleteVisitContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(VisitContactPersonsEntity entity)
    {
        // N�o aplic�vel para VisitContact
        return Task.FromResult(new ValidationResult());
    }
}
