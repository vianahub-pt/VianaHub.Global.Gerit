using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public class ClientContactDataRepository : IClientContactDataRepository
{
    private readonly GeritDbContext _context;

    public ClientContactDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<ClientContactEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<ClientContactEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<ClientContactEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<ClientContactEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<ListPage<ClientContactEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        var query = _context.Set<ClientContactEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(x => 
                EF.Functions.Like(x.Name.ToLower(), $"%{search}%") || 
                EF.Functions.Like(x.Email.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Phone.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Client.Name.ToLower(), $"%{search}%"));
        }

        var count = await query.CountAsync(ct);
        var orderedQuery = CreateSort.ApplyOrdering(query, request);
        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? Paging.MinPageSize();

        var result = await orderedQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new ListPage<ClientContactEntity>
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
        return await _context.Set<ClientContactEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByClientAndEmailAsync(int clientId, string email, int? excludeId, CancellationToken ct)
    {
        var query = _context.Set<ClientContactEntity>()
            .AsNoTracking()
            .Where(x => x.ClientId == clientId && x.Email == email && !x.IsDeleted);

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return await query.AnyAsync(ct);
    }

    public async Task<bool> AddAsync(ClientContactEntity entity, CancellationToken ct)
    {
        await _context.Set<ClientContactEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(ClientContactEntity entity, CancellationToken ct)
    {
        _context.Set<ClientContactEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
