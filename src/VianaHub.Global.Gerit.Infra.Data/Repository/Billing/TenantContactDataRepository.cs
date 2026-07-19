using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Billing;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Billing;

/// <summary>
/// Repositório de dados para TenantContactPersons (contactos de inquilinos).
/// </summary>
public class TenantContactDataRepository : ITenantContactDataRepository
{
    private readonly GeritDbContext _context;

    public TenantContactDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<TenantContactPersonsEntity?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.TenantContacts
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Tenant)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<TenantContactPersonsEntity>> GetByTenantIdAsync(int tenantId, CancellationToken ct = default)
    {
        return await _context.TenantContacts
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Tenant)
            .Where(x => x.TenantId == tenantId && !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<TenantContactPersonsEntity?> GetPrimaryByTenantIdAsync(int tenantId, CancellationToken ct = default)
    {
        return await _context.TenantContacts
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Tenant)
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.IsPrimary && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<TenantContactPersonsEntity>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.TenantContacts
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Tenant)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.TenantId)
            .ThenBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<TenantContactPersonsEntity>> GetActiveAsync(CancellationToken ct = default)
    {
        return await _context.TenantContacts
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Tenant)
            .Where(x => x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.TenantId)
            .ThenBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<ListPage<TenantContactPersonsEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct = default)
    {
        var query = _context.TenantContacts
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Tenant)
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim().ToLower();
            query = query.Where(x =>
                (x.Name != null && EF.Functions.Like(x.Name, $"%{search}%")) ||
                (x.Email != null && EF.Functions.Like(x.Email, $"%{search}%")) ||
                (x.JobTitle != null && EF.Functions.Like(x.JobTitle, $"%{search}%")));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == filter.IsActive.Value);
        }

        var count = await query.CountAsync(ct);
        var orderedQuery = CreateSort.ApplyOrdering(query, filter);
        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? Paging.MinPageSize();

        var result = await orderedQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new ListPage<TenantContactPersonsEntity>
        {
            Items = result,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = count,
            TotalPages = (int)Math.Ceiling((double)count / pageSize)
        };
    }

    public async Task<bool> ExistsByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.TenantContacts
            .AsNoTracking()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsPrimaryContactAsync(int tenantId, CancellationToken ct = default)
    {
        return await _context.TenantContacts
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.IsPrimary && !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(TenantContactPersonsEntity entity, CancellationToken ct = default)
    {
        await _context.TenantContacts.AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(TenantContactPersonsEntity entity, CancellationToken ct = default)
    {
        _context.TenantContacts.Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
