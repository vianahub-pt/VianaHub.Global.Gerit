using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Interfaces.Repository;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository;

public class JwtKeyDataRepository : IJwtKeyDataRepository
{
    private readonly GeritDbContext _context;
    private readonly ILogger<JwtKeyDataRepository> _logger;

    public JwtKeyDataRepository(GeritDbContext context, ILogger<JwtKeyDataRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<JwtKeyEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<JwtKeyEntity>().AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }
    public async Task<JwtKeyEntity> GetByKeyIdAsync(Guid keyId, CancellationToken ct)
    {
        return await _context.Set<JwtKeyEntity>().AsNoTracking()
            .FirstOrDefaultAsync(x => x.KeyId == keyId && !x.IsDeleted, ct);
    }
    public async Task<JwtKeyEntity> GetActiveKeyAsync(int tenantId, CancellationToken ct)
    {
        return await _context.Set<JwtKeyEntity>().AsNoTracking()
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.IsActive && !x.IsDeleted && x.RevokedAt == null, ct);
    }
    public async Task<IEnumerable<JwtKeyEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<JwtKeyEntity>().AsNoTracking()
            .Where(x => !x.IsDeleted).OrderByDescending(x => x.CreatedAt).ToListAsync(ct);
    }
    public async Task<IEnumerable<JwtKeyEntity>> GetByTenantAsync(int tenantId, CancellationToken ct)
    {
        return await _context.Set<JwtKeyEntity>().AsNoTracking()
            .Where(x => x.TenantId == tenantId && !x.IsDeleted).OrderByDescending(x => x.CreatedAt).ToListAsync(ct);
    }
    public async Task<IEnumerable<JwtKeyEntity>> GetKeysEligibleForRotationAsync(CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        return await _context.Set<JwtKeyEntity>().AsNoTracking()
            .Where(x => x.IsActive && !x.IsDeleted && x.RevokedAt == null && x.NextRotationAt <= now).ToListAsync(ct);
    }
    public async Task<IEnumerable<JwtKeyEntity>> GetExpiredKeysAsync(int retentionDays, CancellationToken ct)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-retentionDays);
        return await _context.Set<JwtKeyEntity>().AsNoTracking()
            .Where(x => !x.IsDeleted && !x.IsActive && x.ExpiresAt < cutoffDate).ToListAsync(ct);
    }
    public async Task<bool> HasActiveKeyAsync(int tenantId, Guid applicationId, CancellationToken ct)
    {
        return await _context.Set<JwtKeyEntity>().AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.IsActive && !x.IsDeleted && x.RevokedAt == null, ct);
    }

    public async Task<bool> AddAsync(JwtKeyEntity entity, CancellationToken ct)
    {
        await _context.Set<JwtKeyEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }
    public async Task<bool> UpdateAsync(JwtKeyEntity entity, CancellationToken ct)
    {
        _context.Set<JwtKeyEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
