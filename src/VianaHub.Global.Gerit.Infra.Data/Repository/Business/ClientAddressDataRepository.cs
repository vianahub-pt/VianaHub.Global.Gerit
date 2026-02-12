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
public class ClientAddressDataRepository : IClientAddressDataRepository
{
    private readonly GeritDbContext _context;

    public ClientAddressDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<ClientAddressEntity?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.ClientAddresses
            .AsNoTracking()
            .Where(x => x.Id == id && !x.IsDeleted)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IEnumerable<ClientAddressEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.ClientAddresses
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.ClientId)
            .ThenBy(x => x.IsPrimary ? 0 : 1)
            .ThenBy(x => x.Id)
            .ToListAsync(ct);
    }

    public async Task<ListPage<ClientAddressEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        var query = _context.ClientAddresses
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        // Aplicar busca
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(x => 
                EF.Functions.Like(x.Street.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.City.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.PostalCode.ToLower(), $"%{search}%") ||
                (x.District != null && EF.Functions.Like(x.District.ToLower(), $"%{search}%")) ||
                (x.Neighborhood != null && EF.Functions.Like(x.Neighborhood.ToLower(), $"%{search}%")));
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
            Data = result,
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

    public async Task<bool> ExistsByClientAndAddressTypeAsync(int clientId, int addressTypeId, CancellationToken ct)
    {
        return await _context.ClientAddresses
            .AsNoTracking()
            .AnyAsync(x => x.ClientId == clientId && x.AddressTypeId == addressTypeId && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByClientAndAddressTypeExcludingIdAsync(int clientId, int addressTypeId, int excludeId, CancellationToken ct)
    {
        return await _context.ClientAddresses
            .AsNoTracking()
            .AnyAsync(x => x.ClientId == clientId && x.AddressTypeId == addressTypeId && x.Id != excludeId && !x.IsDeleted, ct);
    }

    public async Task<bool> AddAsync(ClientAddressEntity entity, CancellationToken ct)
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
