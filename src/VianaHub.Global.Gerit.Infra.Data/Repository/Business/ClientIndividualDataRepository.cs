using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public class ClientIndividualDataRepository : IClientIndividualDataRepository
{
    private readonly GeritDbContext _context;

    public ClientIndividualDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<ClientIndividualEntity?> GetByIdAsync(int tenantId, int id, CancellationToken ct = default)
    {
        return await _context.Set<ClientIndividualEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<ClientIndividualEntity?> GetByClientIdAsync(int tenantId, int clientId, CancellationToken ct = default)
    {
        return await _context.Set<ClientIndividualEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.ClientId == clientId && !x.IsDeleted, ct);
    }

    public async Task<ClientIndividualEntity?> GetByDocumentAsync(int tenantId, string documentType, string documentNumber, CancellationToken ct = default)
    {
        return await _context.Set<ClientIndividualEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.DocumentType == documentType && x.DocumentNumber == documentNumber && !x.IsDeleted, ct);
    }

    public async Task<ClientIndividualEntity?> GetByEmailAsync(int tenantId, string email, CancellationToken ct = default)
    {
        return await _context.Set<ClientIndividualEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Email == email && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<ClientIndividualEntity>> GetAllAsync(int tenantId, CancellationToken ct = default)
    {
        return await _context.Set<ClientIndividualEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Where(x => x.TenantId == tenantId && !x.IsDeleted)
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<ClientIndividualEntity>> GetActiveAsync(int tenantId, CancellationToken ct = default)
    {
        return await _context.Set<ClientIndividualEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Where(x => x.TenantId == tenantId && x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .ToListAsync(ct);
    }

    public async Task<ListPage<ClientIndividualEntity>> GetPagedAsync(int tenantId, PagedFilter filter, CancellationToken ct = default)
    {
        var query = _context.Set<ClientIndividualEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Where(x => x.TenantId == tenantId && !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(x =>
                x.FirstName.Contains(filter.Search) ||
                x.LastName.Contains(filter.Search) ||
                x.Email.Contains(filter.Search) ||
                x.DocumentNumber.Contains(filter.Search));
        }

        var totalCount = await query.CountAsync(ct);

        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 10;

        var items = await query
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new ListPage<ClientIndividualEntity>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    public async Task<bool> ExistsByClientIdAsync(int tenantId, int clientId, CancellationToken ct = default)
    {
        return await _context.Set<ClientIndividualEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.ClientId == clientId && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByDocumentAsync(int tenantId, string documentType, string documentNumber, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(documentNumber))
            return false;

        return await _context.Set<ClientIndividualEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.DocumentType == documentType && x.DocumentNumber == documentNumber && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByDocumentAsync(int tenantId, string documentType, string documentNumber, int excludeId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(documentNumber))
            return false;

        return await _context.Set<ClientIndividualEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.DocumentType == documentType && x.DocumentNumber == documentNumber && x.Id != excludeId && !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(ClientIndividualEntity entity, CancellationToken ct = default)
    {
        await _context.Set<ClientIndividualEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(ClientIndividualEntity entity, CancellationToken ct = default)
    {
        _context.Set<ClientIndividualEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
