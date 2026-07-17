using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientAddress;

/// <summary>
/// Validador completo para ClientAddressesEntity
/// </summary>
public class ClientAddressValidator : BaseEntityValidator<ClientAddressesEntity>
{
    public ClientAddressValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(ClientAddressesEntity entity)
    {
        var validator = new CreateClientAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(ClientAddressesEntity entity)
    {
        var validator = new UpdateClientAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(ClientAddressesEntity entity)
    {
        var validator = new ActivateClientAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(ClientAddressesEntity entity)
    {
        var validator = new DeactivateClientAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(ClientAddressesEntity entity)
    {
        var validator = new DeleteClientAddressValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(ClientAddressesEntity entity)
    {
        // ClientAddress n�o tem opera��o de revoke
        return Task.FromResult(new ValidationResult());
    }
}
