using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Billing;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Billing;

/// <summary>
/// Repositório de dados para TenantFiscalData (dados fiscais de inquilinos).
/// </summary>
public class TenantFiscalDataDataRepository : ITenantFiscalDataDataRepository
{
    private readonly GeritDbContext _context;

    public TenantFiscalDataDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<TenantFiscalDataEntity?> GetByIdAsync(int tenantId, int id, CancellationToken ct = default)
    {
        return await _context.TenantFiscalData
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Tenant)
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<TenantFiscalDataEntity>> GetByTenantIdAsync(int tenantId, CancellationToken ct = default)
    {
        return await _context.TenantFiscalData
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Tenant)
            .Where(x => x.TenantId == tenantId && !x.IsDeleted)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<ListPage<TenantFiscalDataEntity>> GetPagedAsync(int tenantId, PagedFilter filter, CancellationToken ct = default)
    {
        var query = _context.TenantFiscalData
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Tenant)
            .Where(x => x.TenantId == tenantId && !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim().ToLower();
            query = query.Where(x =>
                (x.TaxNumber != null && EF.Functions.Like(x.TaxNumber, $"%{search}%")) ||
                (x.VatNumber != null && EF.Functions.Like(x.VatNumber, $"%{search}%")) ||
                (x.FiscalCountry != null && EF.Functions.Like(x.FiscalCountry, $"%{search}%")) ||
                (x.IBAN != null && EF.Functions.Like(x.IBAN, $"%{search}%")) ||
                (x.FiscalEmail != null && EF.Functions.Like(x.FiscalEmail, $"%{search}%")));
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

        return new ListPage<TenantFiscalDataEntity>
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
        return await _context.TenantFiscalData
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsActiveByTenantAsync(int tenantId, CancellationToken ct = default)
    {
        return await _context.TenantFiscalData
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.IsActive && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByTaxNumberAsync(int tenantId, string fiscalCountry, string taxNumber, CancellationToken ct = default)
    {
        return await _context.TenantFiscalData
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.FiscalCountry == fiscalCountry && x.TaxNumber == taxNumber && !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(TenantFiscalDataEntity entity, CancellationToken ct = default)
    {
        await _context.TenantFiscalData.AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(TenantFiscalDataEntity entity, CancellationToken ct = default)
    {
        _context.TenantFiscalData.Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
