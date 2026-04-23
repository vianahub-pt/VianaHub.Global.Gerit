using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Helpers;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

/// <summary>
/// Repositório principal do agregado Client.
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
            .Include(x => x.Individual)
            .Include(x => x.Company)
            .Include(x => x.Contacts)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<ClientEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Clients
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Individual)
            .Include(x => x.Company)
            .Include(x => x.Contacts)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<ListPage<ClientEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        var query = _context.Clients
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Individual)
            .Include(x => x.Company)
            .Include(x => x.Contacts)
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();

            var clientType = ClientTypeHelper.FromDescription(search);
            if (clientType.HasValue)
            {
                query = query.Where(x => x.ClientType == clientType.Value);
            }
            else
            {
                query = query.Where(x =>
                    EF.Functions.Like(x.Individual.FirstName.ToLower(), $"%{search}%") ||
                    EF.Functions.Like(x.Individual.LastName.ToLower(), $"%{search}%") ||
                    EF.Functions.Like(x.Individual.Email.ToLower(), $"%{search}%") ||
                    EF.Functions.Like(x.Individual.Gender.ToLower(), $"%{search}%") ||
                    EF.Functions.Like(x.Individual.Nationality.ToLower(), $"%{search}%") ||
                    EF.Functions.Like(x.Individual.PhoneNumber.ToLower(), $"%{search}%") ||
                    EF.Functions.Like(x.Individual.CellPhoneNumber.ToLower(), $"%{search}%") ||

                    EF.Functions.Like(x.Company.LegalName.ToLower(), $"%{search}%") ||
                    EF.Functions.Like(x.Company.TradeName.ToLower(), $"%{search}%") ||
                    EF.Functions.Like(x.Company.PhoneNumber.ToLower(), $"%{search}%") ||
                    EF.Functions.Like(x.Company.CellPhoneNumber.ToLower(), $"%{search}%") ||
                    EF.Functions.Like(x.Company.Email.ToLower(), $"%{search}%") ||
                    EF.Functions.Like(x.Company.Site.ToLower(), $"%{search}%") ||
                    x.Contacts.Any(c => EF.Functions.Like(c.Name.ToLower(), $"%{search}%") ||
                                        EF.Functions.Like(c.Email.ToLower(), $"%{search}%") ||
                                        EF.Functions.Like(c.PhoneNumber.ToLower(), $"%{search}%")));
            }
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
