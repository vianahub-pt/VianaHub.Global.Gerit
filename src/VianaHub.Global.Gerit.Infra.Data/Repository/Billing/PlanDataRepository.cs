using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Billing;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Billing;

public class PlanDataRepository : IPlanDataRepository
{
    private readonly GeritDbContext _context;

    public PlanDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<PlanEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<PlanEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<PlanEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<PlanEntity>()
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<ListPage<PlanEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        var query = _context.Set<PlanEntity>().AsNoTracking().Where(x => !x.IsDeleted);

        // Filtro de busca
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();

            query = query.Where(x =>
                EF.Functions.Like(x.Name.ToLower(), $"%{search}%")
                || EF.Functions.Like(x.Description.ToLower(), $"%{search}%")
                || EF.Functions.Like(x.Currency.ToLower(), $"%{search}%")
            );
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == request.IsActive.Value);
        }

        var count = await query.CountAsync(ct);

        var orderedQuery = CreateSort.ApplyOrdering(query, request);

        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? Paging.MinPageSize();

        var result = await orderedQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new ListPage<PlanEntity>
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
        return await _context.Set<PlanEntity>().AsNoTracking().AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct)
    {
        return await _context.Set<PlanEntity>().AsNoTracking().AnyAsync(x => x.Name == name && !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(PlanEntity entity, CancellationToken ct)
    {
        await _context.Set<PlanEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(PlanEntity entity, CancellationToken ct)
    {
        _context.Set<PlanEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
