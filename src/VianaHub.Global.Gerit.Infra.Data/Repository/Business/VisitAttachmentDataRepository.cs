using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public class VisitAttachmentDataRepository : IVisitAttachmentDataRepository
{
    private readonly GeritDbContext _context;

    public VisitAttachmentDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<VisitAttachmentEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<VisitAttachmentEntity>()
            .AsNoTracking()
            .Include(x => x.FileType)
            .Include(x => x.AttachmentCategory)
            .Include(x => x.Visit)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<VisitAttachmentEntity> GetByPublicIdAsync(Guid publicId, CancellationToken ct)
    {
        return await _context.Set<VisitAttachmentEntity>()
            .AsNoTracking()
            .Include(x => x.FileType)
            .Include(x => x.AttachmentCategory)
            .FirstOrDefaultAsync(x => x.PublicId == publicId && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<VisitAttachmentEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<VisitAttachmentEntity>()
            .AsNoTracking()
            .Include(x => x.FileType)
            .Include(x => x.AttachmentCategory)
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.IsPrimary)
            .ThenBy(x => x.DisplayOrder)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<VisitAttachmentEntity>> GetByVisitIdAsync(int visitId, CancellationToken ct)
    {
        return await _context.Set<VisitAttachmentEntity>()
            .AsNoTracking()
            .Include(x => x.FileType)
            .Include(x => x.AttachmentCategory)
            .Where(x => x.VisitId == visitId && !x.IsDeleted)
            .OrderByDescending(x => x.IsPrimary)
            .ThenBy(x => x.DisplayOrder)
            .ThenBy(x => x.FileName)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<VisitAttachmentEntity>> GetByCategoryIdAsync(int categoryId, CancellationToken ct)
    {
        return await _context.Set<VisitAttachmentEntity>()
            .AsNoTracking()
            .Include(x => x.FileType)
            .Include(x => x.Visit)
            .Where(x => x.AttachmentCategoryId == categoryId && !x.IsDeleted)
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync(ct);
    }

    public async Task<VisitAttachmentEntity> GetPrimaryByVisitIdAsync(int visitId, CancellationToken ct)
    {
        return await _context.Set<VisitAttachmentEntity>()
            .AsNoTracking()
            .Include(x => x.FileType)
            .Include(x => x.AttachmentCategory)
            .FirstOrDefaultAsync(x => x.VisitId == visitId && x.IsPrimary && x.IsActive && !x.IsDeleted, ct);
    }

    public async Task<ListPage<VisitAttachmentEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        var query = _context.Set<VisitAttachmentEntity>()
            .AsNoTracking()
            .Include(x => x.FileType)
            .Include(x => x.AttachmentCategory)
            .Include(x => x.Visit)
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(x => EF.Functions.Like(x.FileName.ToLower(), $"%{search}%") ||
                                     EF.Functions.Like(x.S3Key.ToLower(), $"%{search}%"));
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

        return new ListPage<VisitAttachmentEntity>
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
        return await _context.Set<VisitAttachmentEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByS3KeyAsync(int tenantId, string s3Key, CancellationToken ct)
    {
        return await _context.Set<VisitAttachmentEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.S3Key == s3Key && !x.IsDeleted, ct);
    }

    public async Task<bool> HasPrimaryAttachmentAsync(int visitId, CancellationToken ct)
    {
        return await _context.Set<VisitAttachmentEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.VisitId == visitId && x.IsPrimary && x.IsActive && !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(VisitAttachmentEntity entity, CancellationToken ct)
    {
        await _context.Set<VisitAttachmentEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(VisitAttachmentEntity entity, CancellationToken ct)
    {
        _context.Set<VisitAttachmentEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<long> GetTotalSizeByVisitIdAsync(int visitId, CancellationToken ct)
    {
        return await _context.Set<VisitAttachmentEntity>()
            .AsNoTracking()
            .Where(x => x.VisitId == visitId && !x.IsDeleted)
            .SumAsync(x => x.FileSizeBytes, ct);
    }

    public async Task<int> GetCountByVisitIdAsync(int visitId, CancellationToken ct)
    {
        return await _context.Set<VisitAttachmentEntity>()
            .AsNoTracking()
            .CountAsync(x => x.VisitId == visitId && !x.IsDeleted, ct);
    }
}
