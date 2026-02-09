using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public class AddressTypeDataRepository : IAddressTypeDataRepository
{
    private readonly GeritDbContext _context;

    public AddressTypeDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<AddressTypeEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<AddressTypeEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<AddressTypeEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<AddressTypeEntity>()
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<ListPage<AddressTypeEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        var query = _context.Set<AddressTypeEntity>()
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(x => EF.Functions.Like(x.Name.ToLower(), $"%{search}%"));
        }

        var count = await query.CountAsync(ct);
        var orderedQuery = CreateSort.ApplyOrdering(query, request);
        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? Paging.MinPageSize();

        var result = await orderedQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new ListPage<AddressTypeEntity>
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
        return await _context.Set<AddressTypeEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct)
    {
        return await _context.Set<AddressTypeEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.Name == name && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByNameAsync(string name, int excludeId, CancellationToken ct)
    {
        return await _context.Set<AddressTypeEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.Name == name && x.Id != excludeId && !x.IsDeleted, ct);
    }

    public async Task AddAsync(AddressTypeEntity entity, CancellationToken ct)
    {
        await _context.Set<AddressTypeEntity>().AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(AddressTypeEntity entity, CancellationToken ct)
    {
        _context.Set<AddressTypeEntity>().Update(entity);
        await _context.SaveChangesAsync(ct);
    }
}
