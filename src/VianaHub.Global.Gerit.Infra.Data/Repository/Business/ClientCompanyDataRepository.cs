using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public class ClientCompanyDataRepository : IClientCompanyDataRepository
{
    private readonly GeritDbContext _context;

    public ClientCompanyDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<ClientCompanyEntity?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Set<ClientCompanyEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<ClientCompanyEntity?> GetByClientIdAsync(int clientId, CancellationToken ct = default)
    {
        return await _context.Set<ClientCompanyEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .FirstOrDefaultAsync(x => x.ClientId == clientId && !x.IsDeleted, ct);
    }

    public async Task<ClientCompanyEntity?> GetByTaxNumberAsync(string taxNumber, CancellationToken ct = default)
    {
        return await _context.Set<ClientCompanyEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .FirstOrDefaultAsync(x => x.CompanyRegistration == taxNumber && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<ClientCompanyEntity>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Set<ClientCompanyEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.LegalName)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<ClientCompanyEntity>> GetActiveAsync(CancellationToken ct = default)
    {
        return await _context.Set<ClientCompanyEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Where(x => x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.LegalName)
            .ToListAsync(ct);
    }

    public async Task<ListPage<ClientCompanyEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct = default)
    {
        var query = _context.Set<ClientCompanyEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(x =>
                x.LegalName.Contains(filter.Search) ||
                x.TradeName.Contains(filter.Search) ||
                x.Email.Contains(filter.Search) ||
                x.CompanyRegistration.Contains(filter.Search));
        }

        var totalCount = await query.CountAsync(ct);

        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 10;

        var items = await query
            .OrderBy(x => x.LegalName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new ListPage<ClientCompanyEntity>
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
        return await _context.Set<ClientCompanyEntity>()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByClientIdAsync(int clientId, CancellationToken ct = default)
    {
        return await _context.Set<ClientCompanyEntity>()
            .AnyAsync(x => x.ClientId == clientId && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByTaxNumberAsync(string taxNumber, CancellationToken ct = default)
    {
        return await _context.Set<ClientCompanyEntity>()
            .AnyAsync(x => x.CompanyRegistration == taxNumber && !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(ClientCompanyEntity entity, CancellationToken ct = default)
    {
        await _context.Set<ClientCompanyEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(ClientCompanyEntity entity, CancellationToken ct = default)
    {
        _context.Set<ClientCompanyEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
