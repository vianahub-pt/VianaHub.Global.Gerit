using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Identity;
using VianaHub.Global.Gerit.Domain.Interfaces.Identity;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Identity;

public class UserRoleDataRepository : IUserRoleDataRepository
{
    private readonly GeritDbContext _context;

    public UserRoleDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<UserRoleEntity> GetByIdAsync(int tenantId, int id, CancellationToken ct)
    {
        return await _context.UserRoles
            .Include(x => x.User)
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Id == id, ct);
    }
    public async Task<IList<UserRoleEntity>> GetByUserAsync(int tenantId, int userId, CancellationToken ct)
    {
        return await _context.UserRoles
            .Include(x => x.Role)
            .Where(x => x.TenantId == tenantId && x.UserId == userId)
            .ToListAsync(ct);
    }

    public async Task<IList<UserRoleEntity>> GetByRoleAsync(int tenantId, int roleId, CancellationToken ct)
    {
        return await _context.UserRoles
            .Include(x => x.User)
            .Where(x => x.TenantId == tenantId && x.RoleId == roleId)
            .ToListAsync(ct);
    }

    public async Task<IList<UserRoleEntity>> GetAllAsync(int tenantId, CancellationToken ct)
    {
        return await _context.UserRoles
            .Include(x => x.User)
            .Include(x => x.Role)
            .Where(x => x.TenantId == tenantId)
            .ToListAsync(ct);
    }
    public async Task<bool> ExistsAsync(int tenantId, int userId, int roleId, CancellationToken ct)
    {
        return await _context.UserRoles.AnyAsync(x => x.TenantId == tenantId && x.UserId == userId && x.RoleId == roleId, ct);
    }

    public async Task AddAsync(UserRoleEntity entity, CancellationToken ct)
    {
        await _context.UserRoles.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int tenantId, int id, CancellationToken ct)
    {
        var entity = await _context.UserRoles.FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Id == id, ct);
        if (entity != null)
        {
            _context.UserRoles.Remove(entity);
            await _context.SaveChangesAsync(ct);
        }
    }
}
