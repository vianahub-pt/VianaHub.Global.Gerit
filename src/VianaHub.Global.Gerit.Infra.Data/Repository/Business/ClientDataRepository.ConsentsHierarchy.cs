using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public partial class ClientDataRepository
{
    async Task<ClientConsentsEntity?> IClientConsentsDataRepository.GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.ConsentType)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    async Task<IEnumerable<ClientConsentsEntity>> IClientConsentsDataRepository.GetByClientIdAsync(int clientId, CancellationToken ct)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.ConsentType)
            .Where(x => x.ClientId == clientId && !x.IsDeleted)
            .OrderByDescending(x => x.GrantedDate)
            .ToListAsync(ct);
    }

    async Task<IEnumerable<ClientConsentsEntity>> IClientConsentsDataRepository.GetByConsentTypeIdAsync(int consentTypeId, CancellationToken ct)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.ConsentType)
            .Where(x => x.ConsentTypeId == consentTypeId && !x.IsDeleted)
            .OrderByDescending(x => x.GrantedDate)
            .ToListAsync(ct);
    }

    async Task<ClientConsentsEntity?> IClientConsentsDataRepository.GetByClientAndConsentTypeAsync(int clientId, int consentTypeId, CancellationToken ct)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.ConsentType)
            .FirstOrDefaultAsync(x => x.ClientId == clientId && x.ConsentTypeId == consentTypeId && !x.IsDeleted, ct);
    }

    async Task<IEnumerable<ClientConsentsEntity>> IClientConsentsDataRepository.GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.ConsentType)
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.GrantedDate)
            .ToListAsync(ct);
    }

    async Task<IEnumerable<ClientConsentsEntity>> IClientConsentsDataRepository.GetActiveAsync(CancellationToken ct)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.ConsentType)
            .Where(x => x.IsActive && !x.IsDeleted)
            .OrderByDescending(x => x.GrantedDate)
            .ToListAsync(ct);
    }

    async Task<IEnumerable<ClientConsentsEntity>> IClientConsentsDataRepository.GetGrantedAsync(CancellationToken ct)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.ConsentType)
            .Where(x => x.Granted && !x.IsDeleted)
            .OrderByDescending(x => x.GrantedDate)
            .ToListAsync(ct);
    }

    async Task<IEnumerable<ClientConsentsEntity>> IClientConsentsDataRepository.GetRevokedAsync(CancellationToken ct)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.ConsentType)
            .Where(x => !x.Granted && x.RevokedDate != null && !x.IsDeleted)
            .OrderByDescending(x => x.RevokedDate)
            .ToListAsync(ct);
    }

    async Task<ListPage<ClientConsentsEntity>> IClientConsentsDataRepository.GetPagedAsync(PagedFilter filter, CancellationToken ct)
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

    async Task<bool> IClientConsentsDataRepository.ExistsByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    async Task<bool> IClientConsentsDataRepository.ExistsByClientAndConsentTypeAsync(int clientId, int consentTypeId, CancellationToken ct)
    {
        return await _context.Set<ClientConsentsEntity>()
            .AnyAsync(x => x.ClientId == clientId && x.ConsentTypeId == consentTypeId && !x.IsDeleted, ct);
    }

    async Task<bool> IClientConsentsDataRepository.AddAsync(ClientConsentsEntity entity, CancellationToken ct)
    {
        await _context.Set<ClientConsentsEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    async Task<bool> IClientConsentsDataRepository.UpdateAsync(ClientConsentsEntity entity, CancellationToken ct)
    {
        _context.Set<ClientConsentsEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    async Task<IEnumerable<ClientHierarchyEntity>> IClientHierarchyDataRepository.GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<ClientHierarchyEntity>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.ParentClient)
            .Include(x => x.ChildClient)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.ParentClient.Name)
            .ThenBy(x => x.ChildClient.Name)
            .ToListAsync(ct);
    }

    async Task<IEnumerable<ClientHierarchyEntity>> IClientHierarchyDataRepository.GetActiveAsync(CancellationToken ct)
    {
        return await _context.Set<ClientHierarchyEntity>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.ParentClient)
            .Include(x => x.ChildClient)
            .Where(x => x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.ParentClient.Name)
            .ThenBy(x => x.ChildClient.Name)
            .ToListAsync(ct);
    }

    async Task<ClientHierarchyEntity> IClientHierarchyDataRepository.GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<ClientHierarchyEntity>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.ParentClient)
            .Include(x => x.ChildClient)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    async Task<IEnumerable<ClientHierarchyEntity>> IClientHierarchyDataRepository.GetByParentClientIdAsync(int parentClientId, CancellationToken ct)
    {
        return await _context.Set<ClientHierarchyEntity>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.ParentClient)
            .Include(x => x.ChildClient)
            .Where(x => x.ParentClientId == parentClientId && !x.IsDeleted)
            .OrderBy(x => x.ChildClient.Name)
            .ToListAsync(ct);
    }

    async Task<IEnumerable<ClientHierarchyEntity>> IClientHierarchyDataRepository.GetByChildClientIdAsync(int childClientId, CancellationToken ct)
    {
        return await _context.Set<ClientHierarchyEntity>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.ParentClient)
            .Include(x => x.ChildClient)
            .Where(x => x.ChildClientId == childClientId && !x.IsDeleted)
            .OrderBy(x => x.ParentClient.Name)
            .ToListAsync(ct);
    }

    async Task<ListPage<ClientHierarchyEntity>> IClientHierarchyDataRepository.GetPagedAsync(PagedFilter filter, CancellationToken ct)
    {
        var query = _context.Set<ClientHierarchyEntity>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.ParentClient)
            .Include(x => x.ChildClient)
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(x =>
                x.ParentClient.Name.Contains(filter.Search) ||
                x.ChildClient.Name.Contains(filter.Search));
        }

        var totalCount = await query.CountAsync(ct);
        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 10;

        var items = await query
            .OrderBy(x => x.ParentClient.Name)
            .ThenBy(x => x.ChildClient.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new ListPage<ClientHierarchyEntity>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    async Task<bool> IClientHierarchyDataRepository.ExistsByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<ClientHierarchyEntity>()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    async Task<bool> IClientHierarchyDataRepository.ExistsRelationshipAsync(int parentClientId, int childClientId, CancellationToken ct)
    {
        return await _context.Set<ClientHierarchyEntity>()
            .AnyAsync(x => x.ParentClientId == parentClientId && x.ChildClientId == childClientId && !x.IsDeleted, ct);
    }

    async Task<bool> IClientHierarchyDataRepository.AddAsync(ClientHierarchyEntity entity, CancellationToken ct)
    {
        await _context.Set<ClientHierarchyEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    async Task<bool> IClientHierarchyDataRepository.UpdateAsync(ClientHierarchyEntity entity, CancellationToken ct)
    {
        _context.Set<ClientHierarchyEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
