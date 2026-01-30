using VianaHub.Global.Gerit.Domain.Entities.Identity;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Identity;

public interface IJwtKeyDataRepository
{
    Task<JwtKeyEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<JwtKeyEntity> GetByKeyIdAsync(Guid keyId, CancellationToken ct);
    Task<JwtKeyEntity> GetActiveKeyAsync(int tenantId, CancellationToken ct);
    Task<IEnumerable<JwtKeyEntity>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<JwtKeyEntity>> GetByTenantAsync(int tenantId, CancellationToken ct);
    Task<IEnumerable<JwtKeyEntity>> GetByApplicationAsync(int tenantId, CancellationToken ct);
    Task<IEnumerable<JwtKeyEntity>> GetKeysEligibleForRotationAsync(CancellationToken ct);
    Task<IEnumerable<JwtKeyEntity>> GetExpiredKeysAsync(int retentionDays, CancellationToken ct);
    Task<ListPage<JwtKeyEntity>> GetPagedAsync(PagedFilter request, int tenantId, CancellationToken ct);
    Task<bool> HasActiveKeyAsync(int tenantId, CancellationToken ct);
    Task<bool> AddAsync(JwtKeyEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(JwtKeyEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(JwtKeyEntity entity, CancellationToken ct);
    Task<int> BulkUpdateTelemetryAsync(List<(int Id, long UsageCount, DateTime? LastUsedAt, long ValidationCount, DateTime? LastValidatedAt)> updates, CancellationToken ct);
}
