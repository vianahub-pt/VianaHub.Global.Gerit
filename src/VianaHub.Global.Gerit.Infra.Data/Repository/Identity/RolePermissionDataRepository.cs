using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Identity;
using VianaHub.Global.Gerit.Domain.Interfaces.Identity;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Identity;

public class RolePermissionDataRepository : IRolePermissionDataRepository
{
    private readonly GeritDbContext _context;

    public RolePermissionDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RolePermissionEntity entity, CancellationToken ct)
    {
        await _context.RolePermissions.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<RolePermissionEntity> GetByIdAsync(int id, int tenantId, CancellationToken ct)
    {
        return await _context.RolePermissions
            .Include(x => x.Role)
            .Include(x => x.Resource)
            .Include(x => x.Action)
            .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId, ct);
    }

    public async Task DeleteAsync(int id, int tenantId, CancellationToken ct)
    {
        var entity = await _context.RolePermissions.FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId, ct);
        if (entity != null)
        {
            _context.RolePermissions.Remove(entity);
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task<IList<RolePermissionEntity>> GetByRoleAsync(int roleId, int tenantId, CancellationToken ct)
    {
        return await _context.RolePermissions
            .Include(x => x.Role)
            .Include(x => x.Resource)
            .Include(x => x.Action)
            .Where(x => x.RoleId == roleId && x.TenantId == tenantId)
            .ToListAsync(ct);
    }

    public async Task<IList<RolePermissionEntity>> GetByResourceAsync(int resourceId, int tenantId, CancellationToken ct)
    {
        return await _context.RolePermissions
            .Include(x => x.Role)
            .Include(x => x.Action)
            .Include(x => x.Resource)
            .Where(x => x.ResourceId == resourceId && x.TenantId == tenantId)
            .ToListAsync(ct);
    }

    public async Task<IList<RolePermissionEntity>> GetAllAsync(int tenantId, CancellationToken ct)
    {
        return await _context.RolePermissions
            .Include(x => x.Role)
            .Include(x => x.Resource)
            .Include(x => x.Action)
            .Where(x => x.TenantId == tenantId)
            .ToListAsync(ct);
    }

    public async Task<bool> ExistsAsync(int tenantId, int roleId, int resourceId, int actionId, CancellationToken ct)
    {
        return await _context.RolePermissions
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.RoleId == roleId && x.ResourceId == resourceId && x.ActionId == actionId, ct);
    }
}
