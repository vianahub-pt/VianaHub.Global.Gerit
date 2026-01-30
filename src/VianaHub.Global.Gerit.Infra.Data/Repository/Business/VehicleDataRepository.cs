using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public class VehicleDataRepository : IVehicleDataRepository
{
    private readonly GeritDbContext _context;

    public VehicleDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<VehicleEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<VehicleEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<VehicleEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<VehicleEntity>().AsNoTracking().Where(x => !x.IsDeleted).OrderBy(x => x.Plate).ToListAsync(ct);
    }

    public async Task<ListPage<VehicleEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        var query = _context.Set<VehicleEntity>().AsNoTracking().Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(x => EF.Functions.Like(x.Plate.ToLower(), $"%{search}%") || EF.Functions.Like(x.Model.ToLower(), $"%{search}%"));
        }

        var count = await query.CountAsync(ct);
        var orderedQuery = CreateSort.ApplyOrdering(query, request);
        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? Paging.MinPageSize();

        var result = await orderedQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new ListPage<VehicleEntity>
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
        return await _context.Set<VehicleEntity>().AsNoTracking().AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByPlateAsync(int tenantId, string plate, CancellationToken ct)
    {
        return await _context.Set<VehicleEntity>().AsNoTracking().AnyAsync(x => x.TenantId == tenantId && x.Plate == plate && !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(VehicleEntity entity, CancellationToken ct)
    {
        await _context.Set<VehicleEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(VehicleEntity entity, CancellationToken ct)
    {
        _context.Set<VehicleEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
