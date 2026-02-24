using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientAddress;

/// <summary>
/// Validador completo para ClientAddressEntity
/// </summary>
public class ClientAddressValidator : BaseEntityValidator<ClientAddressEntity>
{
    public ClientAddressValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(ClientAddressEntity entity)
    {
        var validator = new CreateClientAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(ClientAddressEntity entity)
    {
        var validator = new UpdateClientAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(ClientAddressEntity entity)
    {
        var validator = new ActivateClientAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(ClientAddressEntity entity)
    {
        var validator = new DeactivateClientAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(ClientAddressEntity entity)
    {
        var validator = new DeleteClientAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(ClientAddressEntity entity)
    {
        // ClientAddress não tem operação de revoke
        return Task.FromResult(new ValidationResult());
    }
}
