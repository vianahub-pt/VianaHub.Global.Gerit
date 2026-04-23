using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitAddress;

/// <summary>
/// Validador completo para VisitAddressEntity
/// </summary>
public class VisitAddressValidator : BaseEntityValidator<VisitAddressEntity>
{
    public VisitAddressValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(VisitAddressEntity entity)
    {
        var validator = new CreateVisitAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(VisitAddressEntity entity)
    {
        var validator = new UpdateVisitAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(VisitAddressEntity entity)
    {
        var validator = new ActivateVisitAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(VisitAddressEntity entity)
    {
        var validator = new DeactivateVisitAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(VisitAddressEntity entity)
    {
        var validator = new DeleteVisitAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(VisitAddressEntity entity)
    {
        return Task.FromResult(new ValidationResult());
    }
}
