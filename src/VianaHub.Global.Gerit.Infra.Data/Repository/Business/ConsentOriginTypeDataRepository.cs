using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public class ConsentOriginTypeDataRepository : IConsentOriginTypeDataRepository
{
    private readonly GeritDbContext _context;

    public ConsentOriginTypeDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ConsentOriginTypeEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<ConsentOriginTypeEntity>()
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<ConsentOriginTypeEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<ConsentOriginTypeEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<ListPage<ConsentOriginTypeEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        var query = _context.Set<ConsentOriginTypeEntity>()
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(x =>
                EF.Functions.Like(x.Name.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Description.ToLower(), $"%{search}%")
            );
        }

        if (request.IsActive.HasValue)
            query = query.Where(x => x.IsActive == request.IsActive.Value);

        var count = await query.CountAsync(ct);
        var orderedQuery = CreateSort.ApplyOrdering(query, request);
        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? Paging.MinPageSize();

        var result = await orderedQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new ListPage<ConsentOriginTypeEntity>
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
        return await _context.Set<ConsentOriginTypeEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct)
    {
        return await _context.Set<ConsentOriginTypeEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.Name == name && !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(ConsentOriginTypeEntity entity, CancellationToken ct)
    {
        await _context.Set<ConsentOriginTypeEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(ConsentOriginTypeEntity entity, CancellationToken ct)
    {
        _context.Set<ConsentOriginTypeEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
