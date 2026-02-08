using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public class TeamMemberContactDataRepository : ITeamMemberContactDataRepository
{
    private readonly GeritDbContext _context;

    public TeamMemberContactDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<TeamMemberContactEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<TeamMemberContactEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<TeamMemberContactEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<TeamMemberContactEntity>().AsNoTracking().Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToListAsync(ct);
    }

    public async Task<ListPage<TeamMemberContactEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        var query = _context.Set<TeamMemberContactEntity>().AsNoTracking().Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(x => EF.Functions.Like(x.Name.ToLower(), $"%{search}%") || 
                                     EF.Functions.Like(x.Email.ToLower(), $"%{search}%"));
        }

        var count = await query.CountAsync(ct);
        var orderedQuery = CreateSort.ApplyOrdering(query, request);
        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? Paging.MinPageSize();

        var result = await orderedQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new ListPage<TeamMemberContactEntity>
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
        return await _context.Set<TeamMemberContactEntity>().AsNoTracking().AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByEmailAsync(int tenantId, int teamMemberId, string email, CancellationToken ct)
    {
        return await _context.Set<TeamMemberContactEntity>().AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.TeamMemberId == teamMemberId && x.Email == email && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByEmailForUpdateAsync(int tenantId, int teamMemberId, string email, int excludeId, CancellationToken ct)
    {
        return await _context.Set<TeamMemberContactEntity>().AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.TeamMemberId == teamMemberId && x.Email == email && x.Id != excludeId && !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(TeamMemberContactEntity entity, CancellationToken ct)
    {
        await _context.Set<TeamMemberContactEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(TeamMemberContactEntity entity, CancellationToken ct)
    {
        _context.Set<TeamMemberContactEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
