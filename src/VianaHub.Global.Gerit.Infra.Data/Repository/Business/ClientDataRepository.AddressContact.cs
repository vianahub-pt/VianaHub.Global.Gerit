using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public partial class ClientDataRepository
{
    async Task<ClientAddressEntity> IClientAddressDataRepository.GetByIdAsync(int clientId, int id, CancellationToken ct)
    {
        return await _context.ClientAddresses
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.AddressType)
            .Where(x => x.Client.Id == clientId && x.Id == id && !x.IsDeleted)
            .FirstOrDefaultAsync(ct);
    }

    async Task<IEnumerable<ClientAddressEntity>> IClientAddressDataRepository.GetAllAsync(int clientId, CancellationToken ct)
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

    async Task<ListPage<ClientAddressEntity>> IClientAddressDataRepository.GetPagedAsync(int clientId, PagedFilter request, CancellationToken ct)
    {
        var query = _context.ClientAddresses
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.AddressType)
            .Where(x => x.Client.Id == clientId && !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(x =>
                EF.Functions.Like(x.Street.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.City.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.PostalCode.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.District.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Neighborhood.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Client.Name.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.AddressType.Name.ToLower(), $"%{search}%"));
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == request.IsActive.Value);
        }

        var count = await query.CountAsync(ct);
        var orderedQuery = CreateSort.ApplyOrdering(query, request);
        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? Paging.MinPageSize();

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

    async Task<bool> IClientAddressDataRepository.ExistsByIdAsync(int clientId, int id, CancellationToken ct)
    {
        return await _context.ClientAddresses
            .AsNoTracking()
            .AnyAsync(x => x.Client.Id == clientId && x.Id == id && !x.IsDeleted, ct);
    }

    async Task<bool> IClientAddressDataRepository.ExistsByClientAndAddressTypeAsync(int clientId, int addressTypeId, CancellationToken ct)
    {
        return await _context.ClientAddresses
            .AsNoTracking()
            .AnyAsync(x => x.ClientId == clientId && x.AddressTypeId == addressTypeId && !x.IsDeleted, ct);
    }

    async Task<bool> IClientAddressDataRepository.ExistsByClientAndAddressTypeExcludingIdAsync(int clientId, int addressTypeId, int excludeId, CancellationToken ct)
    {
        return await _context.ClientAddresses
            .AsNoTracking()
            .AnyAsync(x => x.ClientId == clientId && x.AddressTypeId == addressTypeId && x.Id != excludeId && !x.IsDeleted, ct);
    }

    async Task<bool> IClientAddressDataRepository.AddAsync(ClientAddressEntity entity, CancellationToken ct)
    {
        await _context.ClientAddresses.AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    async Task<bool> IClientAddressDataRepository.UpdateAsync(ClientAddressEntity entity, CancellationToken ct)
    {
        _context.ClientAddresses.Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    async Task<bool> IClientAddressDataRepository.DeleteAsync(ClientAddressEntity entity, CancellationToken ct)
    {
        _context.ClientAddresses.Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    async Task<ClientContactEntity> IClientContactDataRepository.GetByIdAsync(int clientId, int id, CancellationToken ct)
    {
        return await _context.Set<ClientContactEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .FirstOrDefaultAsync(x => x.ClientId == clientId && x.Id == id && !x.IsDeleted, ct);
    }

    async Task<IEnumerable<ClientContactEntity>> IClientContactDataRepository.GetAllAsync(int clientId, CancellationToken ct)
    {
        return await _context.Set<ClientContactEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Where(x => x.ClientId == clientId && !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    async Task<ListPage<ClientContactEntity>> IClientContactDataRepository.GetPagedAsync(int clientId, PagedFilter request, CancellationToken ct)
    {
        var query = _context.Set<ClientContactEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Where(x => x.ClientId == clientId && !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(x =>
                EF.Functions.Like(x.Name.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Email.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Phone.ToLower(), $"%{search}%") ||
                EF.Functions.Like(x.Client.Name.ToLower(), $"%{search}%"));
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

        return new ListPage<ClientContactEntity>
        {
            Items = result,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = count,
            TotalPages = (int)Math.Ceiling((double)count / pageSize)
        };
    }

    async Task<bool> IClientContactDataRepository.ExistsByIdAsync(int clientId, int id, CancellationToken ct)
    {
        return await _context.Set<ClientContactEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.ClientId == clientId && x.Id == id && !x.IsDeleted, ct);
    }

    async Task<bool> IClientContactDataRepository.ExistsByClientAndEmailAsync(int clientId, string email, int? excludeId, CancellationToken ct)
    {
        var query = _context.Set<ClientContactEntity>()
            .AsNoTracking()
            .Where(x => x.ClientId == clientId && x.Email == email && !x.IsDeleted);

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return await query.AnyAsync(ct);
    }

    async Task<bool> IClientContactDataRepository.AddAsync(ClientContactEntity entity, CancellationToken ct)
    {
        await _context.Set<ClientContactEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    async Task<bool> IClientContactDataRepository.UpdateAsync(ClientContactEntity entity, CancellationToken ct)
    {
        _context.Set<ClientContactEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
