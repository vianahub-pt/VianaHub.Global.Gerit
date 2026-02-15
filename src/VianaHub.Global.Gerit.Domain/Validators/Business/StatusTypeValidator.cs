using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;
using VianaHub.Global.Gerit.Domain.Validators.Business.StatusType;

namespace VianaHub.Global.Gerit.Domain.Validators.Business;

public class StatusTypeValidator : BaseEntityValidator<StatusTypeEntity>
{
    public StatusTypeValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(StatusTypeEntity entity)
    {
        var validator = new CreateStatusTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(StatusTypeEntity entity)
    {
        var validator = new UpdateStatusTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(StatusTypeEntity entity)
    {
        var validator = new ActivateStatusTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(StatusTypeEntity entity)
    {
        var validator = new DeactivateStatusTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(StatusTypeEntity entity)
    {
        var validator = new DeleteStatusTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(StatusTypeEntity entity)
    {
        return Task.FromResult(new ValidationResult());
    }
}
