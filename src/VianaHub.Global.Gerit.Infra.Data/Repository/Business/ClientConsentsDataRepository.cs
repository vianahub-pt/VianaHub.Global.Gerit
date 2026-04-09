using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public class ClientConsentsDataRepository : IClientConsentsDataRepository
{
    private readonly GeritDbContext _context;

    public ClientConsentsDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<ClientConsentsEntity?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.ConsentType)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<ClientConsentsEntity>> GetByClientIdAsync(int clientId, CancellationToken ct = default)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.ConsentType)
            .Where(x => x.ClientId == clientId && !x.IsDeleted)
            .OrderByDescending(x => x.GrantedDate)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<ClientConsentsEntity>> GetByConsentTypeIdAsync(int consentTypeId, CancellationToken ct = default)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.ConsentType)
            .Where(x => x.ConsentTypeId == consentTypeId && !x.IsDeleted)
            .OrderByDescending(x => x.GrantedDate)
            .ToListAsync(ct);
    }

    public async Task<ClientConsentsEntity?> GetByClientAndConsentTypeAsync(int clientId, int consentTypeId, CancellationToken ct = default)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.ConsentType)
            .FirstOrDefaultAsync(x => x.ClientId == clientId && x.ConsentTypeId == consentTypeId && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<ClientConsentsEntity>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.ConsentType)
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.GrantedDate)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<ClientConsentsEntity>> GetActiveAsync(CancellationToken ct = default)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.ConsentType)
            .Where(x => x.IsActive && !x.IsDeleted)
            .OrderByDescending(x => x.GrantedDate)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<ClientConsentsEntity>> GetGrantedAsync(CancellationToken ct = default)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.ConsentType)
            .Where(x => x.Granted && !x.IsDeleted)
            .OrderByDescending(x => x.GrantedDate)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<ClientConsentsEntity>> GetRevokedAsync(CancellationToken ct = default)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.ConsentType)
            .Where(x => !x.Granted && x.RevokedDate != null && !x.IsDeleted)
            .OrderByDescending(x => x.RevokedDate)
            .ToListAsync(ct);
    }

    public async Task<ListPage<ClientConsentsEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct = default)
    {
        var query = _context.Set<ClientConsentsEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.ConsentType)
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(x =>
                x.Client.Name.Contains(filter.Search) ||
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

    public async Task<bool> ExistsByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
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
