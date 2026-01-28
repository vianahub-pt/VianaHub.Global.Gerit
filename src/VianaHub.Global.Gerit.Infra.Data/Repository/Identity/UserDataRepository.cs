using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Interfaces.Repository;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Identity;

public class UserDataRepository : IUserDataRepository
{
    private readonly GeritDbContext _context;

    public UserDataRepository(GeritDbContext context)
    {
        _context = context;
    }

    public async Task<UserEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<UserEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<UserEntity> GetByEmailAsync(string email, CancellationToken ct)
    {
        var normalizedEmail = email?.ToUpperInvariant();
        return await _context.Set<UserEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.NormalizedEmail == normalizedEmail && !x.IsDeleted, ct);
    }

    public async Task<IEnumerable<UserEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<UserEntity>()
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<ListPage<UserEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        var query = _context.Set<UserEntity>()
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        // ?? Filtro de busca
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();

            query = query.Where(x =>
                EF.Functions.Like(x.Name.ToLower(), $"%{search}%")
                || EF.Functions.Like(x.Email.ToLower(), $"%{search}%")
            );
        }

        var count = await query.CountAsync(ct);

        var orderedQuery = CreateSort.ApplyOrdering(query, request);

        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? Paging.MinPageSize();

        var result = await orderedQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new ListPage<UserEntity>
        {
            Data = result,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = count,
            TotalPages = (int)Math.Ceiling((double)count / pageSize)
        };
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
    {
        var normalizedEmail = email?.ToUpperInvariant();
        return await _context.Set<UserEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.NormalizedEmail == normalizedEmail && !x.IsDeleted, ct);
    }

    public async Task<bool> ExistsByEmailAsync(string email, int excludeId, CancellationToken ct)
    {
        var normalizedEmail = email?.ToUpperInvariant();
        return await _context.Set<UserEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.NormalizedEmail == normalizedEmail && x.Id != excludeId && !x.IsDeleted, ct);
    }

    public async Task<bool> CreateAsync(UserEntity entity, CancellationToken ct)
    {
        await _context.Set<UserEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(UserEntity entity, CancellationToken ct)
    {
        _context.Set<UserEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> DeleteAsync(UserEntity entity, CancellationToken ct)
    {
        _context.Set<UserEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
