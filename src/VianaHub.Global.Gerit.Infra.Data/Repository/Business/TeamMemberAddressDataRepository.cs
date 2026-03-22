using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public class TeamMemberAddressDataRepository : ITeamMemberAddressDataRepository
{
    private readonly GeritDbContext _context;

    public TeamMemberAddressDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<TeamMemberAddressEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.TeamMemberAddresses
            .AsNoTracking()
            .Include(x => x.AddressType)
            .Include(x => x.TeamMember)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<TeamMemberAddressEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.TeamMemberAddresses
            .AsNoTracking()
            .Include(x => x.AddressType)
            .Include(x => x.TeamMember)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.TeamMemberId)
            .ThenBy(x => x.City)
            .ToListAsync(ct);
    }

    public async Task<ListPage<TeamMemberAddressEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        var query = _context.TeamMemberAddresses
            .AsNoTracking()
            .Include(x => x.AddressType)
            .Include(x => x.TeamMember)
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
                EF.Functions.Like(x.TeamMember.Name.ToLower(), $"%{searchLower}%"));
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

        return new ListPage<TeamMemberAddressEntity>
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
        return await _context.TeamMemberAddresses
            .AsNoTracking()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByTeamMemberAndAddressAsync(int tenantId, int teamMemberId, string street, string city, string postalCode, CancellationToken ct)
    {
        return await _context.TeamMemberAddresses
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId &&
                          x.TeamMemberId == teamMemberId &&
                          x.Street == street &&
                          x.City == city &&
                          x.PostalCode == postalCode &&
                          !x.IsDeleted, ct);
    }

    public async Task<TeamMemberAddressEntity> GetPrimaryAddressByTeamMemberAsync(int teamMemberId, CancellationToken ct)
    {
        return await _context.TeamMemberAddresses
            .AsNoTracking()
            .Include(x => x.AddressType)
            .Include(x => x.TeamMember)
            .FirstOrDefaultAsync(x => x.TeamMemberId == teamMemberId && x.IsPrimary && !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(TeamMemberAddressEntity entity, CancellationToken ct)
    {
        await _context.TeamMemberAddresses.AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(TeamMemberAddressEntity entity, CancellationToken ct)
    {
        _context.TeamMemberAddresses.Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
