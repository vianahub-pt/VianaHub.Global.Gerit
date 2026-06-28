using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ConsentOriginType;

public class ConsentOriginTypeValidator : BaseEntityValidator<ConsentOriginTypeEntity>
{
    private readonly ILocalizationService _localization;

    public ConsentOriginTypeValidator(ILocalizationService localization) : base(localization)
    {
        _localization = localization;
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(ConsentOriginTypeEntity entity)
    {
        var validator = new CreateConsentOriginTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(ConsentOriginTypeEntity entity)
    {
        var validator = new UpdateConsentOriginTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(ConsentOriginTypeEntity entity)
    {
        var validator = new ActivateConsentOriginTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(ConsentOriginTypeEntity entity)
    {
        var validator = new DeactivateConsentOriginTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(ConsentOriginTypeEntity entity)
    {
        var validator = new DeleteConsentOriginTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(ConsentOriginTypeEntity entity)
    {
        return Task.FromResult(new ValidationResult());
    }
}
