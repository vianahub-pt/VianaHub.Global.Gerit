using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientContact;

/// <summary>
/// Validador completo para ClientContactPersonsEntity
/// </summary>
public class ClientContactValidator : BaseEntityValidator<ClientContactPersonsEntity>
{
    public ClientContactValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(ClientContactPersonsEntity entity)
    {
        var validator = new CreateClientContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(ClientContactPersonsEntity entity)
    {
        var validator = new UpdateClientContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(ClientContactPersonsEntity entity)
    {
        var validator = new ActivateClientContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(ClientContactPersonsEntity entity)
    {
        var validator = new DeactivateClientContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(ClientContactPersonsEntity entity)
    {
        var validator = new DeleteClientContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(ClientContactPersonsEntity entity)
    {
        // N�o aplic�vel para ClientContact
        return Task.FromResult(new ValidationResult());
    }
}
