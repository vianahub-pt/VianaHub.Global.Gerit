using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public class EmployeeAddressDataRepository : IEmployeeAddressDataRepository
{
    private readonly GeritDbContext _context;

    public EmployeeAddressDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<EmployeeAddressEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.EmployeeAddresses
            .AsNoTracking()
            .Include(x => x.AddressType)
            .Include(x => x.Employee)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<EmployeeAddressEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.EmployeeAddresses
            .AsNoTracking()
            .Include(x => x.AddressType)
            .Include(x => x.Employee)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.EmployeeId)
            .ThenBy(x => x.City)
            .ToListAsync(ct);
    }

    public async Task<ListPage<EmployeeAddressEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        var query = _context.EmployeeAddresses
            .AsNoTracking()
            .Include(x => x.AddressType)
            .Include(x => x.Employee)
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var searchLower = request.Search.ToLower();
            query = query.Where(x =>
                EF.Functions.Like(x.Street.ToLower(), $"%{searchLower}%") ||
                EF.Functions.Like(x.City.ToLower(), $"%{searchLower}%") ||
                EF.Functions.Like(x.PostalCode.ToLower(), $"%{searchLower}%") ||
                EF.Functions.Like(x.District.ToLower(), $"%{searchLower}%") ||
                EF.Functions.Like(x.AddressType.Name.ToLower(), $"%{searchLower}%") ||
                EF.Functions.Like(x.Employee.Name.ToLower(), $"%{searchLower}%"));
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

        return new ListPage<EmployeeAddressEntity>
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
        return await _context.EmployeeAddresses
            .AsNoTracking()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByEmployeeAndAddressAsync(int tenantId, int EmployeeId, string street, string city, string postalCode, CancellationToken ct)
    {
        return await _context.EmployeeAddresses
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId &&
                          x.EmployeeId == EmployeeId &&
                          x.Street == street &&
                          x.City == city &&
                          x.PostalCode == postalCode &&
                          !x.IsDeleted, ct);
    }

    public async Task<EmployeeAddressEntity> GetPrimaryAddressByEmployeeAsync(int EmployeeId, CancellationToken ct)
    {
        return await _context.EmployeeAddresses
            .AsNoTracking()
            .Include(x => x.AddressType)
            .Include(x => x.Employee)
            .FirstOrDefaultAsync(x => x.EmployeeId == EmployeeId && x.IsPrimary && !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(EmployeeAddressEntity entity, CancellationToken ct)
    {
        await _context.EmployeeAddresses.AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(EmployeeAddressEntity entity, CancellationToken ct)
    {
        _context.EmployeeAddresses.Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
