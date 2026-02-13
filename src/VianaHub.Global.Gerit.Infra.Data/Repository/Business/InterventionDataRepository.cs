using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public class InterventionDataRepository : IInterventionDataRepository
{
    private readonly GeritDbContext _context;

    public InterventionDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<InterventionEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<InterventionEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.TeamMember)
            .Include(x => x.Vehicle)
            .Include(x => x.InterventionStatus)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<InterventionEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<InterventionEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.TeamMember)
            .Include(x => x.Vehicle)
            .Include(x => x.InterventionStatus)
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.StartDateTime)
            .ToListAsync(ct);
    }

    public async Task<ListPage<InterventionEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        var query = _context.Set<InterventionEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.TeamMember)
            .Include(x => x.Vehicle)
            .Include(x => x.InterventionStatus)
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(x => 
                EF.Functions.Like(x.Title.ToLower(), $"%{search}%") || 
                EF.Functions.Like(x.Description.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Client.Name.ToLower(), $"%{search}%"));
        }

        var count = await query.CountAsync(ct);
        var orderedQuery = CreateSort.ApplyOrdering(query, request);
        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? Paging.MinPageSize();

        var result = await orderedQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new ListPage<InterventionEntity>
        {
            Data = result,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = count,
            TotalPages = (int)Math.Ceiling((double)count / pageSize)
        };
    }

    public async Task<bool> ExistsByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<InterventionEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByTenantAndTitleAsync(int tenantId, string title, int? excludeId, CancellationToken ct)
    {
        var query = _context.Set<InterventionEntity>()
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId && x.Title == title && !x.IsDeleted);

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return await query.AnyAsync(ct);
    }

    public async Task<bool> AddAsync(InterventionEntity entity, CancellationToken ct)
    {
        await _context.Set<InterventionEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(InterventionEntity entity, CancellationToken ct)
    {
        _context.Set<InterventionEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
