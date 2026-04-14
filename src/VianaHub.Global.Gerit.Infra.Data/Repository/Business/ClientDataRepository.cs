using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public partial class ClientDataRepository :
    IClientDataRepository,
    IClientRepository,
    IClientAddressDataRepository,
    IClientContactDataRepository,
    IClientIndividualDataRepository,
    IClientCompanyDataRepository,
    IClientConsentsDataRepository,
    IClientHierarchyDataRepository,
    IClientIndividualFiscalDataDataRepository,
    IClientCompanyFiscalDataDataRepository
{
    private readonly GeritDbContext _context;

    public ClientDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ClientEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<ClientEntity>()
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<ClientEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<ClientEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<ClientEntity?> GetAggregateByIdAsync(int tenantId, int clientId, CancellationToken ct)
    {
        return await CreateAggregateQuery(trackChanges: false)
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Id == clientId && !x.IsDeleted, ct);
    }

    public async Task<ClientEntity?> GetAggregateForUpdateAsync(int tenantId, int clientId, CancellationToken ct)
    {
        return await CreateAggregateQuery(trackChanges: true)
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Id == clientId && !x.IsDeleted, ct);
    }


    public async Task<IEnumerable<ClientEntity>> GetAllAsync(int tenantId, CancellationToken ct)
    {
        return await _context.Set<ClientEntity>()
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId && !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<ListPage<ClientEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        var query = _context.Set<ClientEntity>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Contacts)
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(x =>
                EF.Functions.Like(x.Name.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Email.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Phone.ToLower(), $"%{search}%") ||
                x.Contacts.Any(c =>
                    EF.Functions.Like(c.Name.ToLower(), $"%{search}%") ||
                    EF.Functions.Like(c.Email.ToLower(), $"%{search}%") ||
                    EF.Functions.Like(c.Phone.ToLower(), $"%{search}%")));
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == request.IsActive.Value);
        }

        var count = await query.CountAsync(ct);
        var orderedQuery = CreateSort.ApplyOrdering(query, request);
        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? Paging.MinPageSize();

        var result = await orderedQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new ListPage<ClientEntity>
        {
            Items = result,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = count,
            TotalPages = (int)Math.Ceiling((double)count / pageSize)
        };
    }

    public async Task<ListPage<ClientEntity>> GetPagedAsync(int tenantId, PagedFilter request, CancellationToken ct)
    {
        var query = _context.Set<ClientEntity>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Contacts)
            .Where(x => x.TenantId == tenantId && !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(x =>
                EF.Functions.Like(x.Name.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Email.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Phone.ToLower(), $"%{search}%") ||
                x.Contacts.Any(c =>
                    EF.Functions.Like(c.Name.ToLower(), $"%{search}%") ||
                    EF.Functions.Like(c.Email.ToLower(), $"%{search}%") ||
                    EF.Functions.Like(c.Phone.ToLower(), $"%{search}%")));
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == request.IsActive.Value);
        }

        var count = await query.CountAsync(ct);
        var orderedQuery = CreateSort.ApplyOrdering(query, request);
        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? Paging.MinPageSize();

        var result = await orderedQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new ListPage<ClientEntity>
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
        return await _context.Set<ClientEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByIdAsync(int tenantId, int clientId, CancellationToken ct)
    {
        return await _context.Set<ClientEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.Id == clientId && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByEmailAsync(int tenantId, string email, CancellationToken ct)
    {
        return await _context.Set<ClientEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.Email == email && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByEmailForUpdateAsync(int tenantId, string email, int excludeId, CancellationToken ct)
    {
        return await _context.Set<ClientEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.Email == email && x.Id != excludeId && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByEmailAsync(int tenantId, string email, int? excludeClientId, CancellationToken ct)
    {
        var query = _context.Set<ClientEntity>()
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId && x.Email == email && !x.IsDeleted);

        if (excludeClientId.HasValue)
        {
            query = query.Where(x => x.Id != excludeClientId.Value);
        }

        return await query.AnyAsync(ct);
    }

    public async Task<bool> ExistsIndividualDocumentAsync(int tenantId, string documentType, string documentNumber, int? excludeClientId, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(documentNumber))
        {
            return false;
        }

        var query = _context.Set<ClientIndividualEntity>()
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId && x.DocumentType == documentType && x.DocumentNumber == documentNumber && !x.IsDeleted);

        if (excludeClientId.HasValue)
        {
            query = query.Where(x => x.ClientId != excludeClientId.Value);
        }

        return await query.AnyAsync(ct);
    }

    public async Task<bool> ExistsCompanyRegistrationAsync(int tenantId, string companyRegistration, int? excludeClientId, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(companyRegistration))
        {
            return false;
        }

        var query = _context.Set<ClientCompanyEntity>()
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId && x.CompanyRegistration == companyRegistration && !x.IsDeleted);

        if (excludeClientId.HasValue)
        {
            query = query.Where(x => x.ClientId != excludeClientId.Value);
        }

        return await query.AnyAsync(ct);
    }

    public async Task<bool> AddAsync(ClientEntity entity, CancellationToken ct)
    {
        await _context.Set<ClientEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(ClientEntity entity, CancellationToken ct)
    {
        return await _context.SaveChangesAsync(ct) > 0;
    }

    private IQueryable<ClientEntity> CreateAggregateQuery(bool trackChanges)
    {
        var query = _context.Set<ClientEntity>()
            .AsSplitQuery()
            .Include(x => x.Contacts)
            .Include(x => x.Addresses)
                .ThenInclude(x => x.AddressType)
            .Include(x => x.Consents)
                .ThenInclude(x => x.ConsentType)
            .Include(x => x.Individual)
                .ThenInclude(x => x.FiscalData)
            .Include(x => x.Company)
                .ThenInclude(x => x.FiscalData)
            .Include(x => x.ChildHierarchies)
                .ThenInclude(x => x.ChildClient)
            .Include(x => x.ParentHierarchies)
                .ThenInclude(x => x.ParentClient);

        return trackChanges ? query : query.AsNoTracking();
    }
}

