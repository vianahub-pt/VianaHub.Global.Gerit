using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientContact;

/// <summary>
/// Validador completo para ClientContactEntity
/// </summary>
public class ClientContactValidator : BaseEntityValidator<ClientContactEntity>
{
    public ClientContactValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(ClientContactEntity entity)
    {
        var validator = new CreateClientContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(ClientContactEntity entity)
    {
        var validator = new UpdateClientContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(ClientContactEntity entity)
    {
        var validator = new ActivateClientContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(ClientContactEntity entity)
    {
        var validator = new DeactivateClientContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(ClientContactEntity entity)
    {
        var validator = new DeleteClientContactValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(ClientContactEntity entity)
    {
        // Não aplicável para ClientContact
        return Task.FromResult(new ValidationResult());
    }
}
