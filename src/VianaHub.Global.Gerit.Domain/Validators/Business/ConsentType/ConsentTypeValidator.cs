using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ConsentType;

public class ConsentTypeValidator : BaseEntityValidator<ConsentTypeEntity>
{
    private readonly ILocalizationService _localization;

    public ConsentTypeValidator(ILocalizationService localization) : base(localization)
    {
        _localization = localization;
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(ConsentTypeEntity entity)
    {
        var validator = new CreateConsentTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(ConsentTypeEntity entity)
    {
        var validator = new UpdateConsentTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(ConsentTypeEntity entity)
    {
        var validator = new ActivateConsentTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(ConsentTypeEntity entity)
    {
        var validator = new DeactivateConsentTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(ConsentTypeEntity entity)
    {
        var validator = new DeleteConsentTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(ConsentTypeEntity entity)
    {
        return Task.FromResult(new ValidationResult());
    }
}
