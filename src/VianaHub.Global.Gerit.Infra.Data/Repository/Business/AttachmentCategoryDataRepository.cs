using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public class AttachmentCategoryDataRepository : IAttachmentCategoryDataRepository
{
    private readonly GeritDbContext _context;

    public AttachmentCategoryDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<AttachmentCategoryEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<AttachmentCategoryEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<AttachmentCategoryEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<AttachmentCategoryEntity>()
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<AttachmentCategoryEntity>> GetActiveAsync(CancellationToken ct)
    {
        return await _context.Set<AttachmentCategoryEntity>()
            .AsNoTracking()
            .Where(x => x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<ListPage<AttachmentCategoryEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        var query = _context.Set<AttachmentCategoryEntity>()
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(x => EF.Functions.Like(x.Name.ToLower(), $"%{search}%") ||
                                     EF.Functions.Like(x.Description.ToLower(), $"%{search}%"));
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == request.IsActive.Value);
        }

        var count = await query.CountAsync(ct);
        var orderedQuery = CreateSort.ApplyOrdering(query, request);
        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? Paging.MinPageSize();

        var result = await orderedQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new ListPage<AttachmentCategoryEntity>
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
        return await _context.Set<AttachmentCategoryEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByNameAsync(int tenantId, string name, CancellationToken ct)
    {
        return await _context.Set<AttachmentCategoryEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.Name == name && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByNameAsync(int tenantId, string name, int excludeId, CancellationToken ct)
    {
        return await _context.Set<AttachmentCategoryEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.Name == name && x.Id != excludeId && !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(AttachmentCategoryEntity entity, CancellationToken ct)
    {
        await _context.Set<AttachmentCategoryEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(AttachmentCategoryEntity entity, CancellationToken ct)
    {
        _context.Set<AttachmentCategoryEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
