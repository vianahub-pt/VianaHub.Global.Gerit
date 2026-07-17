using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientDocument;

/// <summary>
/// Validador completo para ClientDocumentsEntity
/// </summary>
public class ClientDocumentValidator : BaseEntityValidator<ClientDocumentsEntity>
{
    public ClientDocumentValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(ClientDocumentsEntity entity)
    {
        var validator = new CreateClientDocumentValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(ClientDocumentsEntity entity)
    {
        var validator = new UpdateClientDocumentValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(ClientDocumentsEntity entity)
    {
        var validator = new ActivateClientDocumentValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(ClientDocumentsEntity entity)
    {
        var validator = new DeactivateClientDocumentValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(ClientDocumentsEntity entity)
    {
        var validator = new DeleteClientDocumentValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(ClientDocumentsEntity entity)
    {
        // Não aplicável para ClientDocument
        return Task.FromResult(new ValidationResult());
    }
}
