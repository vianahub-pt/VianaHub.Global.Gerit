using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

/// <summary>
/// Repositório principal do agregado Client unificado.
/// </summary>
public class ClientRepository : IClientRepository
{
    private readonly GeritDbContext _context;

    public ClientRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ClientEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Clients
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.PartyType)
            .ThenInclude(x => x.Translations)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<ClientEntity?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Clients
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.PartyType)
            .ThenInclude(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<ListPage<ClientEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        var query = _context.Clients
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.FiscalData)
            .Include(x => x.Contacts)
            .Include(x => x.Addresses)
            .Include(x => x.PartyType)
            .ThenInclude(x => x.Translations)
            .Include(x => x.AcquisitionSourceType)
            .ThenInclude(x => x.Translations)
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();

            query = query.Where(x =>
                EF.Functions.Like(x.Name.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Email.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.PhoneNumber.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.CellPhoneNumber.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Gender.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Nationality.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.CompanyRegistrationNumber.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.EconomicActivityCode.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.WebsiteUrl.ToLower(), $"%{search}%") ||
                x.Contacts.Any(c =>
                    EF.Functions.Like(c.Name.ToLower(), $"%{search}%") ||
                    EF.Functions.Like(c.Email.ToLower(), $"%{search}%") ||
                    EF.Functions.Like(c.PhoneNumber.ToLower(), $"%{search}%")));
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == request.IsActive.Value && !x.IsDeleted);
        }

        var count = await query.CountAsync(ct);
        var orderedQuery = CreateSort.ApplyOrdering(query, request);
        
        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? Paging.MinPageSize();
        var skip = (pageNumber - 1) * pageSize;

        var result = await orderedQuery.Skip(skip).Take(pageSize).ToListAsync(ct);

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
        return await _context.Clients
            .AsNoTracking()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(ClientEntity entity, CancellationToken ct)
    {
        await _context.Clients.AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(ClientEntity entity, CancellationToken ct)
    {
        _context.Clients.Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
