using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Interfaces.Repository;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository;

public class RefreshTokenDataRepository : IRefreshTokenDataRepository
{
    private readonly GeritDbContext _context;

    public RefreshTokenDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RefreshTokenEntity entity)
    {
        await _context.Set<RefreshTokenEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshTokenEntity> GetByTokenAsync(string token, int tenantId)
    {
        return await _context.Set<RefreshTokenEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Token == token && x.TenantId == tenantId);
    }

    public async Task<IEnumerable<RefreshTokenEntity>> GetByUserAsync(int userId, int tenantId)
    {
        return await _context.Set<RefreshTokenEntity>()
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.TenantId == tenantId)
            .ToListAsync();
    }

    public async Task RevokeAsync(string token, int revokedBy, int tenantId)
    {
        var entity = await _context.Set<RefreshTokenEntity>()
            .FirstOrDefaultAsync(x => x.Token == token && x.TenantId == tenantId);

        if (entity == null)
            return;

        entity.Revoke(revokedBy);
        _context.Set<RefreshTokenEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }
}
