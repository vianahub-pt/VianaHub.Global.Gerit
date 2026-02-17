using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

/// <summary>
/// Repositório de dados para InterventionAddress
/// </summary>
public class InterventionAddressDataRepository : IInterventionAddressDataRepository
{
    private readonly GeritDbContext _context;

    public InterventionAddressDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<InterventionAddressEntity?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.InterventionAddresses
            .AsNoTracking()
            .Include(x => x.Intervention)
            .Include(x => x.AddressType)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<InterventionAddressEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.InterventionAddresses
            .AsNoTracking()
            .Include(x => x.Intervention)
            .Include(x => x.AddressType)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.InterventionId)
            .ThenBy(x => x.City)
            .ToListAsync(ct);
    }

    public async Task<ListPage<InterventionAddressEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct)
    {
        var query = _context.InterventionAddresses
            .AsNoTracking()
            .Include(x => x.Intervention)
            .Include(x => x.AddressType)
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var searchLower = filter.Search.ToLower();
            query = query.Where(x =>
                EF.Functions.Like(x.Street.ToLower(), $"%{searchLower}%") ||
                EF.Functions.Like(x.City.ToLower(), $"%{searchLower}%") ||
                EF.Functions.Like(x.PostalCode.ToLower(), $"%{searchLower}%") ||
                EF.Functions.Like(x.District.ToLower(), $"%{searchLower}%") ||
                EF.Functions.Like(x.Neighborhood.ToLower(), $"%{searchLower}%") ||
                EF.Functions.Like(x.Intervention.Title.ToLower(), $"%{searchLower}%") ||
                EF.Functions.Like(x.AddressType.Name.ToLower(), $"%{searchLower}%"));
        }

        var count = await query.CountAsync(ct);
        var orderedQuery = CreateSort.ApplyOrdering(query, filter);
        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? Paging.MinPageSize();

        var result = await orderedQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new ListPage<InterventionAddressEntity>
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
        return await _context.InterventionAddresses
            .AsNoTracking()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByInterventionAndAddressAsync(int tenantId, int interventionId, string street, string city, string postalCode, CancellationToken ct)
    {
        return await _context.InterventionAddresses
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId &&
                          x.InterventionId == interventionId &&
                          x.Street == street &&
                          x.City == city &&
                          x.PostalCode == postalCode &&
                          !x.IsDeleted, ct);
    }

    public async Task<InterventionAddressEntity> GetPrimaryAddressByInterventionAsync(int interventionId, CancellationToken ct)
    {
        return await _context.InterventionAddresses
            .AsNoTracking()
            .Include(x => x.AddressType)
            .Include(x => x.Intervention)
            .FirstOrDefaultAsync(x => x.InterventionId == interventionId && x.IsPrimary && !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(InterventionAddressEntity entity, CancellationToken ct)
    {
        await _context.InterventionAddresses.AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(InterventionAddressEntity entity, CancellationToken ct)
    {
        _context.InterventionAddresses.Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
