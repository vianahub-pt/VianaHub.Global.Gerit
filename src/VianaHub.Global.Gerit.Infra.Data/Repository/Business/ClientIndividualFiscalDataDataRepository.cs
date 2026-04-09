using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public class ClientIndividualFiscalDataDataRepository : IClientIndividualFiscalDataDataRepository
{
    private readonly GeritDbContext _context;

    public ClientIndividualFiscalDataDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<ClientIndividualFiscalDataEntity?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Set<ClientIndividualFiscalDataEntity>()
            .AsNoTracking()
            .Include(x => x.ClientIndividual)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<ClientIndividualFiscalDataEntity?> GetByClientIndividualIdAsync(int clientIndividualId, CancellationToken ct = default)
    {
        return await _context.Set<ClientIndividualFiscalDataEntity>()
            .AsNoTracking()
            .Include(x => x.ClientIndividual)
            .FirstOrDefaultAsync(x => x.ClientIndividualId == clientIndividualId && !x.IsDeleted, ct);
    }

    public async Task<ClientIndividualFiscalDataEntity?> GetByTaxNumberAsync(string taxNumber, CancellationToken ct = default)
    {
        return await _context.Set<ClientIndividualFiscalDataEntity>()
            .AsNoTracking()
            .Include(x => x.ClientIndividual)
            .FirstOrDefaultAsync(x => x.TaxNumber == taxNumber && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<ClientIndividualFiscalDataEntity>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Set<ClientIndividualFiscalDataEntity>()
            .AsNoTracking()
            .Include(x => x.ClientIndividual)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.TaxNumber)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<ClientIndividualFiscalDataEntity>> GetActiveAsync(CancellationToken ct = default)
    {
        return await _context.Set<ClientIndividualFiscalDataEntity>()
            .AsNoTracking()
            .Include(x => x.ClientIndividual)
            .Where(x => x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.TaxNumber)
            .ToListAsync(ct);
    }

    public async Task<ListPage<ClientIndividualFiscalDataEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct = default)
    {
        var query = _context.Set<ClientIndividualFiscalDataEntity>()
            .AsNoTracking()
            .Include(x => x.ClientIndividual)
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(x =>
                x.TaxNumber.Contains(filter.Search) ||
                x.VatNumber.Contains(filter.Search) ||
                x.FiscalEmail.Contains(filter.Search));
        }

        var totalCount = await query.CountAsync(ct);

        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 10;

        var items = await query
            .OrderBy(x => x.TaxNumber)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new ListPage<ClientIndividualFiscalDataEntity>
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
        return await _context.Set<ClientIndividualFiscalDataEntity>()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByClientIndividualIdAsync(int clientIndividualId, CancellationToken ct = default)
    {
        return await _context.Set<ClientIndividualFiscalDataEntity>()
            .AnyAsync(x => x.ClientIndividualId == clientIndividualId && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByTaxNumberAsync(string taxNumber, int? excludeId = null, CancellationToken ct = default)
    {
        var query = _context.Set<ClientIndividualFiscalDataEntity>()
            .Where(x => x.TaxNumber == taxNumber && !x.IsDeleted);

        if (excludeId.HasValue)
            query = query.Where(x => x.Id != excludeId.Value);

        return await query.AnyAsync(ct);
    }

    public async Task<bool> AddAsync(ClientIndividualFiscalDataEntity entity, CancellationToken ct = default)
    {
        await _context.Set<ClientIndividualFiscalDataEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(ClientIndividualFiscalDataEntity entity, CancellationToken ct = default)
    {
        _context.Set<ClientIndividualFiscalDataEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
