using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public class InterventionContactDataRepository : IInterventionContactDataRepository
{
    private readonly GeritDbContext _context;

    public InterventionContactDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<InterventionContactEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<InterventionContactEntity>()
            .AsNoTracking()
            .Include(x => x.Intervention)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<InterventionContactEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<InterventionContactEntity>()
            .AsNoTracking()
            .Include(x => x.Intervention)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<ListPage<InterventionContactEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        var query = _context.Set<InterventionContactEntity>()
            .AsNoTracking()
            .Include(x => x.Intervention)
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(x => 
                EF.Functions.Like(x.Name.ToLower(), $"%{search}%") || 
                EF.Functions.Like(x.Email.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Phone.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Intervention.Title.ToLower(), $"%{search}%"));
        }

        var count = await query.CountAsync(ct);
        var orderedQuery = CreateSort.ApplyOrdering(query, request);
        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? Paging.MinPageSize();

        var result = await orderedQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new ListPage<InterventionContactEntity>
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
        return await _context.Set<InterventionContactEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByInterventionAndEmailAsync(int interventionId, string email, int? excludeId, CancellationToken ct)
    {
        var query = _context.Set<InterventionContactEntity>()
            .AsNoTracking()
            .Where(x => x.InterventionId == interventionId && x.Email == email && !x.IsDeleted);

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return await query.AnyAsync(ct);
    }

    public async Task<bool> AddAsync(InterventionContactEntity entity, CancellationToken ct)
    {
        await _context.Set<InterventionContactEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(InterventionContactEntity entity, CancellationToken ct)
    {
        _context.Set<InterventionContactEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
