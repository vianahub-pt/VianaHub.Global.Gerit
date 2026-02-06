using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public class TeamMemberDataRepository : ITeamMemberDataRepository
{
    private readonly GeritDbContext _context;

    public TeamMemberDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<TeamMemberEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<TeamMemberEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<TeamMemberEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<TeamMemberEntity>().AsNoTracking().Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToListAsync(ct);
    }

    public async Task<ListPage<TeamMemberEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        var query = _context.Set<TeamMemberEntity>().AsNoTracking().Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(x => EF.Functions.Like(x.Name.ToLower(), $"%{search}%") || 
                                     EF.Functions.Like(x.TaxNumber.ToLower(), $"%{search}%"));
        }

        var count = await query.CountAsync(ct);
        var orderedQuery = CreateSort.ApplyOrdering(query, request);
        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? Paging.MinPageSize();

        var result = await orderedQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new ListPage<TeamMemberEntity>
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
        return await _context.Set<TeamMemberEntity>().AsNoTracking().AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByTaxNumberAsync(int tenantId, string taxNumber, CancellationToken ct)
    {
        return await _context.Set<TeamMemberEntity>().AsNoTracking().AnyAsync(x => x.TenantId == tenantId && x.TaxNumber == taxNumber && !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(TeamMemberEntity entity, CancellationToken ct)
    {
        await _context.Set<TeamMemberEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(TeamMemberEntity entity, CancellationToken ct)
    {
        _context.Set<TeamMemberEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
