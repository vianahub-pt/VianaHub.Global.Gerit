using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Infra.Data.Context;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using System;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public class InterventionTeamDataRepository : IInterventionTeamDataRepository
{
    private readonly GeritDbContext _context;

    public InterventionTeamDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<InterventionTeamEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<InterventionTeamEntity>()
            .AsNoTracking()
            .Include(x => x.Team)
            .Include(x => x.Intervention)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<InterventionTeamEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<InterventionTeamEntity>()
            .AsNoTracking()
            .Include(x => x.Team)
            .Include(x => x.Intervention)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Id)
            .ToListAsync(ct);
    }

    public async Task<ListPage<InterventionTeamEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        var query = _context.Set<InterventionTeamEntity>()
            .AsNoTracking()
            .Include(x => x.Team)
            .Include(x => x.Intervention)
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(x => 
                EF.Functions.Like(x.Team.Name.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Intervention.Title.ToLower(), $"%{search}%"));
        }

        var count = await query.CountAsync(ct);

        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? Paging.MinPageSize();

        var result = await query.OrderByDescending(x => x.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new ListPage<InterventionTeamEntity>
        {
            Data = result,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = count,
            TotalPages = (int)Math.Ceiling((double)count / pageSize)
        };
    }

    public async Task<bool> ExistsByIdAsync(int tenantId, int interventionId, int teamId, CancellationToken ct)
    {
        return await _context.Set<InterventionTeamEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.InterventionId == interventionId && x.TeamId == teamId &&  !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(InterventionTeamEntity entity, CancellationToken ct)
    {
        await _context.Set<InterventionTeamEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(InterventionTeamEntity entity, CancellationToken ct)
    {
        _context.Set<InterventionTeamEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
