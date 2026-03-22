using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Infra.Data.Context;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using System;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public class InterventionTeamEquipmentDataRepository : IInterventionTeamEquipmentDataRepository
{
    private readonly GeritDbContext _context;

    public InterventionTeamEquipmentDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<InterventionTeamEquipmentEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<InterventionTeamEquipmentEntity>()
            .AsNoTracking()
            .Include(x => x.Equipment)
            .Include(x => x.InterventionTeam)
            .Include(x => x.InterventionTeam.Intervention)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<InterventionTeamEquipmentEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<InterventionTeamEquipmentEntity>()
            .AsNoTracking()
            .Include(x => x.Equipment)
            .Include(x => x.InterventionTeam)
            .Include(x => x.InterventionTeam.Intervention)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Id)
            .ToListAsync(ct);
    }

    public async Task<ListPage<InterventionTeamEquipmentEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        var query = _context.Set<InterventionTeamEquipmentEntity>()
            .AsNoTracking()
            .Include(x => x.Equipment)
            .Include(x => x.InterventionTeam)
            .Include(x => x.InterventionTeam.Intervention)
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(x => 
                EF.Functions.Like(x.Equipment.Name.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.InterventionTeam.Intervention.Title.ToLower(), $"%{search}%"));
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == request.IsActive.Value);
        }

        var count = await query.CountAsync(ct);

        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? Paging.MinPageSize();

        var result = await query.OrderByDescending(x => x.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new ListPage<InterventionTeamEquipmentEntity>
        {
            Items = result,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = count,
            TotalPages = (int)Math.Ceiling((double)count / pageSize)
        };
    }

    public async Task<bool> ExistsByIdAsync(int tenantId, int interventionTeamId, int equipmentId, CancellationToken ct)
    {
        return await _context.Set<InterventionTeamEquipmentEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.InterventionTeamId == interventionTeamId && x.EquipmentId == equipmentId &&  !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(InterventionTeamEquipmentEntity entity, CancellationToken ct)
    {
        await _context.Set<InterventionTeamEquipmentEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(InterventionTeamEquipmentEntity entity, CancellationToken ct)
    {
        _context.Set<InterventionTeamEquipmentEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
