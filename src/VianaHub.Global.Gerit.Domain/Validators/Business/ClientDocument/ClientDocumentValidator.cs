using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientDocument;

/// <summary>
/// Validador completo para ClientDocumentEntity
/// </summary>
public class ClientDocumentValidator : BaseEntityValidator<ClientDocumentEntity>
{
    public ClientDocumentValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(ClientDocumentEntity entity)
    {
        var validator = new CreateClientDocumentValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(ClientDocumentEntity entity)
    {
        var validator = new UpdateClientDocumentValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(ClientDocumentEntity entity)
    {
        var validator = new ActivateClientDocumentValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(ClientDocumentEntity entity)
    {
        var validator = new DeactivateClientDocumentValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(ClientDocumentEntity entity)
    {
        var validator = new DeleteClientDocumentValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(ClientDocumentEntity entity)
    {
        // Não aplicável para ClientDocument
        return Task.FromResult(new ValidationResult());
    }
}
