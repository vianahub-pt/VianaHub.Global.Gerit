using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.Client;

/// <summary>
/// Validador completo para ClientEntity
/// </summary>
public class ClientValidator : BaseEntityValidator<ClientEntity>
{
    public ClientValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(ClientEntity entity)
    {
        var validator = new CreateClientValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(ClientEntity entity)
    {
        var validator = new UpdateClientValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(ClientEntity entity)
    {
        var validator = new ActivateClientValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(ClientEntity entity)
    {
        var validator = new DeactivateClientValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(ClientEntity entity)
    {
        var validator = new DeleteClientValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(ClientEntity entity)
    {
        // Não aplicável para Client
        return Task.FromResult(new ValidationResult());
    }
}
