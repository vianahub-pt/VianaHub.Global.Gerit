using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

/// <summary>
/// Repositório de dados para ClientAddress
/// </summary>
public class ClientAddressRepository : IClientAddressDataRepository
{
    private readonly GeritDbContext _context;

    public ClientAddressRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<ClientAddressEntity> GetByIdAsync(int clientId, int id, CancellationToken ct)
    {
        return await _context.ClientAddresses
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.AddressType)
            .Where(x => x.ClientId == clientId && x.Id == id && !x.IsDeleted)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IEnumerable<ClientAddressEntity>> GetAllAsync(int clientId, CancellationToken ct)
    {
        return await _context.ClientAddresses
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.AddressType)
            .Where(x => x.Client.Id == clientId && !x.IsDeleted)
            .OrderBy(x => x.ClientId)
            .ThenBy(x => x.IsPrimary ? 0 : 1)
            .ThenBy(x => x.Id)
            .ToListAsync(ct);
    }

    public async Task<ListPage<ClientAddressEntity>> GetPagedAsync(int clientId, PagedFilter request, CancellationToken ct)
    {
        var query = _context.ClientAddresses
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.AddressType)
            .Include(x => x.Client.Individual)
            .Include(x => x.Client.Company)
            .Where(x => x.Client.Id == clientId && !x.IsDeleted);

        // Aplicar busca
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(x => 
                EF.Functions.Like(x.Street.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.City.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.PostalCode.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.District.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Neighborhood.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Client.Individual.FirstName.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Client.Individual.LastName.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Client.Company.LegalName.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.AddressType.Name.ToLower(), $"%{search}%"));
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == request.IsActive.Value);
        }

        // Total de registros
        var count = await query.CountAsync(ct);

        // Aplicar ordenação
        var orderedQuery = CreateSort.ApplyOrdering(query, request);
        
        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? Paging.MinPageSize();

        // Aplicar paginação
        var result = await orderedQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new ListPage<ClientAddressEntity>
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
        return await _context.ClientAddresses
            .AsNoTracking()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByClientIdAsync(int clientId, string countryCode, string street, string streetNumber, string neighborhood, string city, string district, string postalCode, CancellationToken ct)
    {
        return await _context.ClientAddresses
            .AsNoTracking()
            .AnyAsync(x => x.Client.Id == clientId && 
                           x.CountryCode == countryCode &&
                           x.Street == street &&
                           x.StreetNumber == streetNumber &&
                           x.Neighborhood == neighborhood &&
                           x.City == city &&
                           x.District == district &&
                           x.PostalCode == postalCode &&
                           !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByClientAndAddressTypeAsync(int clientId, int addressTypeId, CancellationToken ct)
    {
        return await _context.ClientAddresses
            .AsNoTracking()
            .AnyAsync(x => x.ClientId == clientId && x.AddressTypeId == addressTypeId && !x.IsDeleted, ct);
    }

    public async Task<bool> CreateAsync(ClientAddressEntity entity, CancellationToken ct)
    {
        await _context.ClientAddresses.AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(ClientAddressEntity entity, CancellationToken ct)
    {
        _context.ClientAddresses.Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> DeleteAsync(ClientAddressEntity entity, CancellationToken ct)
    {
        _context.ClientAddresses.Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
