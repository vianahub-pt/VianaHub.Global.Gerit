using VianaHub.Global.Gerit.Domain.Entities;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Repository;

public interface IJwtKeyDataRepository
{
    Task<JwtKeyEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<JwtKeyEntity> GetByKeyIdAsync(Guid keyId, CancellationToken ct);
    Task<JwtKeyEntity> GetActiveKeyAsync(int tenantId, CancellationToken ct);
    Task<IEnumerable<JwtKeyEntity>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<JwtKeyEntity>> GetByTenantAsync(int tenantId, CancellationToken ct);
    Task<IEnumerable<JwtKeyEntity>> GetKeysEligibleForRotationAsync(CancellationToken ct);
    Task<IEnumerable<JwtKeyEntity>> GetExpiredKeysAsync(int retentionDays, CancellationToken ct);
    Task<bool> HasActiveKeyAsync(int tenantId, Guid applicationId, CancellationToken ct);
    Task<bool> AddAsync(JwtKeyEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(JwtKeyEntity entity, CancellationToken ct);
}
