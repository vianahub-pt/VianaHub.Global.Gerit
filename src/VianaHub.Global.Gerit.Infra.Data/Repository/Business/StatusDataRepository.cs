using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

/// <summary>
/// Repositório de dados para Status
/// </summary>
public class StatusDataRepository : IStatusDataRepository
{
    private readonly GeritDbContext _context;

    public StatusDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<StatusEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<StatusEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<StatusEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<StatusEntity>()
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<ListPage<StatusEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        var query = _context.Set<StatusEntity>()
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(x => 
                EF.Functions.Like(x.Name.ToLower(), $"%{search}%") || 
                EF.Functions.Like(x.Description.ToLower(), $"%{search}%"));
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == request.IsActive.Value);
        }

        var count = await query.CountAsync(ct);
        var orderedQuery = CreateSort.ApplyOrdering(query, request);
        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? Paging.MinPageSize();

        var result = await orderedQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new ListPage<StatusEntity>
        {
            Items = result,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = count,
            TotalPages = (int)Math.Ceiling((double)count / pageSize)
        };
    }

    public async Task<bool> ExistsByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<StatusEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByNameAsync(int tenantId, string name, CancellationToken ct)
    {
        return await _context.Set<StatusEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.Name == name && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByNameForUpdateAsync(int tenantId, string name, int id, CancellationToken ct)
    {
        return await _context.Set<StatusEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.Name == name && x.Id != id && !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(StatusEntity entity, CancellationToken ct)
    {
        await _context.Set<StatusEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(StatusEntity entity, CancellationToken ct)
    {
        _context.Set<StatusEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
