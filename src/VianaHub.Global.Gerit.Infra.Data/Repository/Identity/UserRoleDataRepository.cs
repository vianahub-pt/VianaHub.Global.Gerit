using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    public async Task AddAsync(UserRoleEntity entity)
    {
        await _context.UserRoles.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<UserRoleEntity> GetByIdAsync(int id, int tenantId)
    {
        return await _context.UserRoles
            .Include(x => x.User)
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId);
    }

    public async Task DeleteAsync(int id, int tenantId)
    {
        var entity = await _context.UserRoles.FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId);
        if (entity != null)
        {
            _context.UserRoles.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IList<UserRoleEntity>> GetByUserAsync(int userId, int tenantId)
    {
        return await _context.UserRoles
            .Include(x => x.Role)
            .Where(x => x.UserId == userId && x.TenantId == tenantId)
            .ToListAsync();
    }

    public async Task<IList<UserRoleEntity>> GetByRoleAsync(int roleId, int tenantId)
    {
        return await _context.UserRoles
            .Include(x => x.User)
            .Where(x => x.RoleId == roleId && x.TenantId == tenantId)
            .ToListAsync();
    }

    public async Task<IList<UserRoleEntity>> GetAllAsync(int tenantId)
    {
        return await _context.UserRoles
            .Include(x => x.User)
            .Include(x => x.Role)
            .Where(x => x.TenantId == tenantId)
            .ToListAsync();
    }
}
