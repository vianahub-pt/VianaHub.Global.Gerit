using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public class ClientContactRepository : IClientContactDataRepository
{
    private readonly GeritDbContext _context;

    public ClientContactRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ClientContactEntity>> GetAllAsync(int clientId, CancellationToken ct)
    {
        return await _context.ClientContacts
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Client)
            .ThenInclude(x => x.Individual)
            .Include(x => x.Client)
            .ThenInclude(x => x.Company)
            .Where(x => x.ClientId == clientId && !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<ClientContactEntity> GetByIdAsync(int clientId, int id, CancellationToken ct)
    {
        return await _context.ClientContacts
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Client)
            .ThenInclude(x => x.Individual)
            .Include(x => x.Client)
            .ThenInclude(x => x.Company)
            .FirstOrDefaultAsync(x => x.ClientId == clientId && x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<ListPage<ClientContactEntity>> GetPagedAsync(int clientId, PagedFilter filter, CancellationToken ct)
    {
        var query = _context.ClientContacts
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Client)
            .ThenInclude(x => x.Individual)
            .Include(x => x.Client)
            .ThenInclude(x => x.Company)
            .Where(x => x.ClientId == clientId && !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim().ToLower();
            query = query.Where(x => 
                (x.Name != null && EF.Functions.Like(x.Name, $"%{search}%")) || 
                (x.Email != null && EF.Functions.Like(x.Email, $"%{search}%")));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == filter.IsActive.Value);
        }

        var count = await query.CountAsync(ct);
        var orderedQuery = CreateSort.ApplyOrdering(query, filter);
        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? Paging.MinPageSize();

        var result = await orderedQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new ListPage<ClientContactEntity>
        {
            Items = result,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = count,
            TotalPages = (int)Math.Ceiling((double)count / pageSize)
        };
    }

    public async Task<bool> ExistsByClientAndEmailAsync(int clientId, string name, string email, CancellationToken ct)
    {
        return await _context.ClientContacts
            .AsNoTracking()
            .AnyAsync(x => x.Client.Id == clientId &&
                           x.Name == name &&
                           x.Email == email &&
                           !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(ClientContactEntity entity, CancellationToken ct)
    {
        await _context.ClientContacts
            .AddAsync(entity, ct);

        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(ClientContactEntity entity, CancellationToken ct)
    {
        _context.ClientContacts
            .Update(entity);

        return await _context.SaveChangesAsync(ct) > 0;
    }
}
