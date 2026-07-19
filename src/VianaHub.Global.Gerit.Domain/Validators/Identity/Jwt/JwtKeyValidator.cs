using FluentValidation.Results;
using FluentValidation;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Identity;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Identity.Jwt;

/// <summary>
/// Valida��es de dom�nio para entidades de chaves JWT.
/// Implementa��o simples que garante campos essenciais e segue o padr�o de retorno via ValidationResult.
/// </summary>
public class JwtKeyValidator : IEntityDomainValidator<JwtKeysEntity>
{
    private readonly ILocalizationService _localization;

    public JwtKeyValidator(ILocalizationService localization)
    {
        _localization = localization;
    }

    public Task<ValidationResult> ValidateForCreateAsync(JwtKeysEntity entity)
    {
        var vr = new ValidationResult();

        if (entity == null)
        {
            vr.Errors.Add(new ValidationFailure("JwtKey", _localization.GetMessage("Domain.JwtKey.EntityRequired")));
            return Task.FromResult(vr);
        }

        if (entity.TenantId <= 0)
            vr.Errors.Add(new ValidationFailure(nameof(entity.TenantId), _localization.GetMessage("Domain.JwtKey.TenantRequired")));

        if (string.IsNullOrWhiteSpace(entity.PublicKey))
            vr.Errors.Add(new ValidationFailure(nameof(entity.PublicKey), _localization.GetMessage("Domain.JwtKey.PublicKeyRequired")));

        if (string.IsNullOrWhiteSpace(entity.PrivateKeyEncrypted))
            vr.Errors.Add(new ValidationFailure(nameof(entity.PrivateKeyEncrypted), _localization.GetMessage("Domain.JwtKey.PrivateKeyRequired")));

        if (string.IsNullOrWhiteSpace(entity.Algorithm))
            vr.Errors.Add(new ValidationFailure(nameof(entity.Algorithm), _localization.GetMessage("Domain.JwtKey.AlgorithmRequired")));

        if (entity.KeySize < 1024)
            vr.Errors.Add(new ValidationFailure(nameof(entity.KeySize), _localization.GetMessage("Domain.JwtKey.KeySizeInvalid")));

        return Task.FromResult(vr);
    }

    public Task<ValidationResult> ValidateForUpdateAsync(JwtKeysEntity entity)
    {
        // Atualiza��o b�sica: n�o permite alterar TenantId/KeyId/public key via update
        var vr = new ValidationResult();
        if (entity == null)
        {
            vr.Errors.Add(new ValidationFailure("JwtKey", _localization.GetMessage("Domain.JwtKey.EntityRequired")));
            return Task.FromResult(vr);
        }

        if (entity.TenantId <= 0)
            vr.Errors.Add(new ValidationFailure(nameof(entity.TenantId), _localization.GetMessage("Domain.JwtKey.TenantRequired")));

        return Task.FromResult(vr);
    }

    public Task<ValidationResult> ValidateForActivateAsync(JwtKeysEntity entity)
    {
        var vr = new ValidationResult();
        if (entity == null)
        {
            vr.Errors.Add(new ValidationFailure("JwtKey", _localization.GetMessage("Domain.JwtKey.EntityRequired")));
            return Task.FromResult(vr);
        }

        if (entity.IsDeleted)
            vr.Errors.Add(new ValidationFailure(nameof(entity.IsDeleted), _localization.GetMessage("Domain.JwtKey.CannotActivateDeleted")));

        return Task.FromResult(vr);
    }

    public Task<ValidationResult> ValidateForDeactivateAsync(JwtKeysEntity entity)
    {
        // Sem regras adicionais por enquanto
        return Task.FromResult(new ValidationResult());
    }

    public Task<ValidationResult> ValidateForDeleteAsync(JwtKeysEntity entity)
    {
        var vr = new ValidationResult();
        if (entity == null)
        {
            vr.Errors.Add(new ValidationFailure("JwtKey", _localization.GetMessage("Domain.JwtKey.EntityRequired")));
            return Task.FromResult(vr);
        }

        if (entity.IsActive)
            vr.Errors.Add(new ValidationFailure(nameof(entity.IsActive), _localization.GetMessage("Domain.JwtKey.CannotDeleteActive")));

        return Task.FromResult(vr);
    }

    public Task<ValidationResult> ValidateForRevokeAsync(JwtKeysEntity entity)
    {
        var vr = new ValidationResult();
        if (entity == null)
        {
            vr.Errors.Add(new ValidationFailure("JwtKey", _localization.GetMessage("Domain.JwtKey.EntityRequired")));
            return Task.FromResult(vr);
        }

        if (entity.IsRevoked())
            vr.Errors.Add(new ValidationFailure("IsRevoked", _localization.GetMessage("Domain.JwtKey.AlreadyRevoked")));

        return Task.FromResult(vr);
    }
}
