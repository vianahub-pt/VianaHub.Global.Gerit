using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Billing;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Billing;

/// <summary>
/// Repositório de dados para TenantAddresses (endereços de inquilinos).
/// </summary>
public class TenantAddressesDataRepository : ITenantAddressesDataRepository
{
    private readonly GeritDbContext _context;

    public TenantAddressesDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<TenantAddressesEntity?> GetByIdAsync(int tenantId, int id, CancellationToken ct = default)
    {
        return await _context.TenantAddresses
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Tenant)
            .Include(x => x.AddressType)
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<TenantAddressesEntity>> GetByTenantIdAsync(int tenantId, CancellationToken ct = default)
    {
        return await _context.TenantAddresses
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Tenant)
            .Include(x => x.AddressType)
            .Where(x => x.TenantId == tenantId && !x.IsDeleted)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<ListPage<TenantAddressesEntity>> GetPagedAsync(int tenantId, PagedFilter filter, CancellationToken ct = default)
    {
        var query = _context.TenantAddresses
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Tenant)
            .Include(x => x.AddressType)
            .Where(x => x.TenantId == tenantId && !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim().ToLower();
            query = query.Where(x =>
                (x.Street != null && EF.Functions.Like(x.Street, $"%{search}%")) ||
                (x.Neighborhood != null && EF.Functions.Like(x.Neighborhood, $"%{search}%")) ||
                (x.City != null && EF.Functions.Like(x.City, $"%{search}%")) ||
                (x.District != null && EF.Functions.Like(x.District, $"%{search}%")) ||
                (x.PostalCode != null && EF.Functions.Like(x.PostalCode, $"%{search}%")) ||
                (x.CountryCode != null && EF.Functions.Like(x.CountryCode, $"%{search}%")));
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

        return new ListPage<TenantAddressesEntity>
        {
            Items = result,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = count,
            TotalPages = (int)Math.Ceiling((double)count / pageSize)
        };
    }

    public async Task<bool> ExistsByIdAsync(int tenantId, int id, CancellationToken ct = default)
    {
        return await _context.TenantAddresses
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsPrimaryByTenantAsync(int tenantId, CancellationToken ct = default)
    {
        return await _context.TenantAddresses
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.IsPrimary && x.IsActive && !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(TenantAddressesEntity entity, CancellationToken ct = default)
    {
        await _context.TenantAddresses.AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(TenantAddressesEntity entity, CancellationToken ct = default)
    {
        _context.TenantAddresses.Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
