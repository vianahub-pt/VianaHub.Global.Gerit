using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public class StatusTypeDataRepository : IStatusTypeDataRepository
{
    private readonly GeritDbContext _context;

    public StatusTypeDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StatusTypeEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.StatusTypes
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<StatusTypeEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.StatusTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<ListPage<StatusTypeEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct)
    {
        var query = _context.StatusTypes
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim().ToLower();
            query = query.Where(x => 
                EF.Functions.Like(x.Name.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Description.ToLower(), $"%{search}%"));
        }

        var count = await query.CountAsync(ct);
        var orderedQuery = CreateSort.ApplyOrdering(query, filter);
        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? Paging.MinPageSize();

        var result = await orderedQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new ListPage<StatusTypeEntity>
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
        return await _context.StatusTypes
            .AsNoTracking()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByNameAsync(int tenantId, string name, CancellationToken ct)
    {
        return await _context.StatusTypes
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.Name == name && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByNameForUpdateAsync(int tenantId, string name, int excludeId, CancellationToken ct)
    {
        return await _context.StatusTypes
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.Name == name && x.Id != excludeId && !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(StatusTypeEntity entity, CancellationToken ct)
    {
        await _context.StatusTypes.AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(StatusTypeEntity entity, CancellationToken ct)
    {
        _context.StatusTypes.Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
