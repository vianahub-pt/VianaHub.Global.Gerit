using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientType;

public class ClientTypeValidator : BaseEntityValidator<ClientTypeEntity>
{
    private readonly ILocalizationService _localization;

    public ClientTypeValidator(ILocalizationService localization) : base(localization)
    {
        _localization = localization;
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(ClientTypeEntity entity)
    {
        var validator = new CreateClientTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(ClientTypeEntity entity)
    {
        var validator = new UpdateClientTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(ClientTypeEntity entity)
    {
        var validator = new ActivateClientTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(ClientTypeEntity entity)
    {
        var validator = new DeactivateClientTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(ClientTypeEntity entity)
    {
        var validator = new DeleteClientTypeValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(ClientTypeEntity entity)
    {
        return Task.FromResult(new ValidationResult());
    }
}
