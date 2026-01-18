using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Interfaces.Repository;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository;

public class RolePermissionDataRepository : IRolePermissionDataRepository
{
    private readonly GeritDbContext _context;

    public RolePermissionDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RolePermissionEntity entity)
    {
        await _context.RolePermissions.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<RolePermissionEntity> GetByIdAsync(int id, int tenantId)
    {
        return await _context.RolePermissions
            .Include(x => x.Role)
            .Include(x => x.Resource)
            .Include(x => x.Action)
            .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId);
    }

    public async Task DeleteAsync(int id, int tenantId)
    {
        var entity = await _context.RolePermissions.FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId);
        if (entity != null)
        {
            _context.RolePermissions.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IList<RolePermissionEntity>> GetByRoleAsync(int roleId, int tenantId)
    {
        return await _context.RolePermissions
            .Include(x => x.Resource)
            .Include(x => x.Action)
            .Where(x => x.RoleId == roleId && x.TenantId == tenantId)
            .ToListAsync();
    }

    public async Task<IList<RolePermissionEntity>> GetByResourceAsync(int resourceId, int tenantId)
    {
        return await _context.RolePermissions
            .Include(x => x.Role)
            .Include(x => x.Action)
            .Where(x => x.ResourceId == resourceId && x.TenantId == tenantId)
            .ToListAsync();
    }

    public async Task<IList<RolePermissionEntity>> GetAllAsync(int tenantId)
    {
        return await _context.RolePermissions
            .Include(x => x.Role)
            .Include(x => x.Resource)
            .Include(x => x.Action)
            .Where(x => x.TenantId == tenantId)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(int tenantId, int roleId, int resourceId, int actionId)
    {
        return await _context.RolePermissions
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.RoleId == roleId && x.ResourceId == resourceId && x.ActionId == actionId);
    }
}
