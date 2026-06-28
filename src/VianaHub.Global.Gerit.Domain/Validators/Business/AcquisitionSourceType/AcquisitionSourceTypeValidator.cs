using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.AcquisitionSourceType;

public class AcquisitionSourceTypeValidator : BaseEntityValidator<AcquisitionSourceTypeEntity>
{
    private readonly ILocalizationService _localization;

    public AcquisitionSourceTypeValidator(ILocalizationService localization) : base(localization)
    {
        _localization = localization;
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(AcquisitionSourceTypeEntity entity)
    {
        var validator = new CreateAcquisitionSourceTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(AcquisitionSourceTypeEntity entity)
    {
        var validator = new UpdateAcquisitionSourceTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(AcquisitionSourceTypeEntity entity)
    {
        var validator = new ActivateAcquisitionSourceTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(AcquisitionSourceTypeEntity entity)
    {
        var validator = new DeactivateAcquisitionSourceTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(AcquisitionSourceTypeEntity entity)
    {
        var validator = new DeleteAcquisitionSourceTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(AcquisitionSourceTypeEntity entity)
    {
        return Task.FromResult(new ValidationResult());
    }
}
