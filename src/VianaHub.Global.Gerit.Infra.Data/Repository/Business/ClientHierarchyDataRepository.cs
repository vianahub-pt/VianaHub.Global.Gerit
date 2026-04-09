using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public class ClientHierarchyDataRepository : IClientHierarchyDataRepository
{
    private readonly GeritDbContext _context;

    public ClientHierarchyDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<ClientHierarchyEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<ClientHierarchyEntity>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.ParentClient)
            .Include(x => x.ChildClient)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<ClientHierarchyEntity>> GetByParentClientIdAsync(int parentClientId, CancellationToken ct)
    {
        return await _context.Set<ClientHierarchyEntity>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.ParentClient)
            .Include(x => x.ChildClient)
            .Where(x => x.ParentClientId == parentClientId && !x.IsDeleted)
            .OrderBy(x => x.ChildClient.Name)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<ClientHierarchyEntity>> GetByChildClientIdAsync(int childClientId, CancellationToken ct)
    {
        return await _context.Set<ClientHierarchyEntity>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.ParentClient)
            .Include(x => x.ChildClient)
            .Where(x => x.ChildClientId == childClientId && !x.IsDeleted)
            .OrderBy(x => x.ParentClient.Name)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<ClientHierarchyEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<ClientHierarchyEntity>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.ParentClient)
            .Include(x => x.ChildClient)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.ParentClient.Name)
            .ThenBy(x => x.ChildClient.Name)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<ClientHierarchyEntity>> GetActiveAsync(CancellationToken ct)
    {
        return await _context.Set<ClientHierarchyEntity>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.ParentClient)
            .Include(x => x.ChildClient)
            .Where(x => x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.ParentClient.Name)
            .ThenBy(x => x.ChildClient.Name)
            .ToListAsync(ct);
    }

    public async Task<ListPage<ClientHierarchyEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct)
    {
        var query = _context.Set<ClientHierarchyEntity>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.ParentClient)
            .Include(x => x.ChildClient)
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(x =>
                x.ParentClient.Name.Contains(filter.Search) ||
                x.ChildClient.Name.Contains(filter.Search));
        }

        var totalCount = await query.CountAsync(ct);

        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 10;

        var items = await query
            .OrderBy(x => x.ParentClient.Name)
            .ThenBy(x => x.ChildClient.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new ListPage<ClientHierarchyEntity>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    public async Task<bool> ExistsByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<ClientHierarchyEntity>()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsRelationshipAsync(int parentClientId, int childClientId, CancellationToken ct)
    {
        return await _context.Set<ClientHierarchyEntity>()
            .AnyAsync(x => x.ParentClientId == parentClientId && x.ChildClientId == childClientId && !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(ClientHierarchyEntity entity, CancellationToken ct)
    {
        await _context.Set<ClientHierarchyEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(ClientHierarchyEntity entity, CancellationToken ct)
    {
        _context.Set<ClientHierarchyEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }

}
