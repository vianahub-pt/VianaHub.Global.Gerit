using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public class ClientConsentRepository : IClientConsentsDataRepository
{
    private readonly GeritDbContext _context;

    public ClientConsentRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ClientConsentsEntity>> GetAllAsync(int clientId, CancellationToken ct = default)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Client)
            .ThenInclude(x => x.Individual)
            .Include(x => x.Client)
            .ThenInclude(x => x.Company)
            .Where(x => x.ClientId == clientId && !x.IsDeleted)
            .OrderByDescending(x => x.GrantedDate)
            .ToListAsync(ct);
    }

    public async Task<ClientConsentsEntity> GetByIdAsync(int clientId, int id, CancellationToken ct = default)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Client)
            .ThenInclude(x => x.Individual)
            .Include(x => x.Client)
            .ThenInclude(x => x.Company)
            .Where(x => x.ClientId == clientId && x.Id == id && !x.IsDeleted)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<ListPage<ClientConsentsEntity>> GetPagedAsync(int clientId, PagedFilter filter, CancellationToken ct = default)
    {
        var query = _context.Set<ClientConsentsEntity>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Client)
            .ThenInclude(x => x.Individual)
            .Include(x => x.Client)
            .ThenInclude(x => x.Company)
            .Where(x => x.ClientId == clientId && !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(x =>
                x.ConsentType.Name.Contains(filter.Search) ||
                x.Origin.Contains(filter.Search));
        }

        var totalCount = await query.CountAsync(ct);

        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 10;

        var items = await query
            .OrderByDescending(x => x.GrantedDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new ListPage<ClientConsentsEntity>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    public async Task<bool> ExistsByIdAsync(int clientId, int id, CancellationToken ct = default)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AnyAsync(x => x.ClientId == clientId && x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByClientAndConsentTypeAsync(int clientId, int consentTypeId, CancellationToken ct = default)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AnyAsync(x => x.ClientId == clientId && x.ConsentTypeId == consentTypeId && !x.IsDeleted, ct);
    }
    
    public async Task<bool> AddAsync(ClientConsentsEntity entity, CancellationToken ct = default)
    {
        await _context.Set<ClientConsentsEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(ClientConsentsEntity entity, CancellationToken ct = default)
    {
        _context.Set<ClientConsentsEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
