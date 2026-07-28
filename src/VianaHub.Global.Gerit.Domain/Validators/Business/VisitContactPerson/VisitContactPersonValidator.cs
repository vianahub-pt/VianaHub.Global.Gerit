using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitContactPersons;

/// <summary>
/// Validador completo para VisitContactPersonsEntity
/// </summary>
public class VisitContactPersonValidator : BaseEntityValidator<VisitContactPersonsEntity>
{
    public VisitContactPersonValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(VisitContactPersonsEntity entity)
    {
        var validator = new CreateVisitContactPersonValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(VisitContactPersonsEntity entity)
    {
        var validator = new UpdateVisitContactPersonValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(VisitContactPersonsEntity entity)
    {
        var validator = new ActivateVisitContactPersonValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(VisitContactPersonsEntity entity)
    {
        var validator = new DeactivateVisitContactPersonValidator(_localization);
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
