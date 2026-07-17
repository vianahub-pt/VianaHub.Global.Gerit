using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitAddress;

/// <summary>
/// Validador completo para VisitAddressesEntity
/// </summary>
public class VisitAddressValidator : BaseEntityValidator<VisitAddressesEntity>
{
    public VisitAddressValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(VisitAddressesEntity entity)
    {
        var validator = new CreateVisitAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(VisitAddressesEntity entity)
    {
        var validator = new UpdateVisitAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(VisitAddressesEntity entity)
    {
        var validator = new ActivateVisitAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(VisitAddressesEntity entity)
    {
        var validator = new DeactivateVisitAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(VisitAddressesEntity entity)
    {
        var validator = new DeleteVisitAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(VisitAddressesEntity entity)
    {
        return Task.FromResult(new ValidationResult());
    }
}
