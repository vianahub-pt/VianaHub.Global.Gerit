using VianaHub.Global.Gerit.Domain.Entities.Identity;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Identity;

public interface IJwtKeyDomainService
{
    Task<JwtKeyEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<JwtKeyEntity> GetByKeyIdAsync(Guid keyId, CancellationToken ct);
    Task<JwtKeyEntity> GetActiveKeyAsync(int tenantId, CancellationToken ct);
    Task<IEnumerable<JwtKeyEntity>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<JwtKeyEntity>> GetByTenantAsync(int tenantId, CancellationToken ct);
    Task<JwtKeyEntity> CreateAsync(JwtKeyEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(JwtKeyEntity key, CancellationToken ct);
    Task<bool> DeactivateAsync(JwtKeyEntity key, CancellationToken ct);
    Task<bool> RevokeAsync(int id, string reason, int modifiedBy, CancellationToken ct);
    Task<bool> DeleteAsync(JwtKeyEntity key, CancellationToken ct);
    Task<bool> UpdateRotationPolicyAsync(int id, int rotationPolicyDays, int overlapPeriodDays, int modifiedBy, CancellationToken ct);
    Task<(string PublicKey, string PrivateKeyEncrypted)> GenerateKeyPairAsync(string algorithm, int keySize, CancellationToken ct);
    Task<string> DecryptPrivateKeyAsync(string encryptedPrivateKey, CancellationToken ct);
    Task<int> RotateKeysAsync(CancellationToken ct);
    Task<int> CleanupExpiredKeysAsync(int retentionDays, CancellationToken ct);
    Task<JwtKeyEntity> EnsureKeyExistsAsync(int tenantId, int createdBy, CancellationToken ct);
}
