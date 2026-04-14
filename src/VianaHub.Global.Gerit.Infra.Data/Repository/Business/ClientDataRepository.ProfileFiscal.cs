using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Infra.Data.Repository.Business;

public partial class ClientDataRepository
{
    async Task<ClientIndividualEntity?> IClientIndividualDataRepository.GetByIdAsync(int tenantId, int id, CancellationToken ct)
    {
        return await _context.Set<ClientIndividualEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.FiscalData)
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Id == id && !x.IsDeleted, ct);
    }

    async Task<ClientIndividualEntity?> IClientIndividualDataRepository.GetByClientIdAsync(int tenantId, int clientId, CancellationToken ct)
    {
        return await _context.Set<ClientIndividualEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.FiscalData)
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.ClientId == clientId && !x.IsDeleted, ct);
    }

    async Task<ClientIndividualEntity?> IClientIndividualDataRepository.GetByDocumentAsync(int tenantId, string documentType, string documentNumber, CancellationToken ct)
    {
        return await _context.Set<ClientIndividualEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.FiscalData)
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.DocumentType == documentType && x.DocumentNumber == documentNumber && !x.IsDeleted, ct);
    }

    async Task<ClientIndividualEntity?> IClientIndividualDataRepository.GetByEmailAsync(int tenantId, string email, CancellationToken ct)
    {
        return await _context.Set<ClientIndividualEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.FiscalData)
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Email == email && !x.IsDeleted, ct);
    }

    async Task<IEnumerable<ClientIndividualEntity>> IClientIndividualDataRepository.GetAllAsync(int tenantId, CancellationToken ct)
    {
        return await _context.Set<ClientIndividualEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.FiscalData)
            .Where(x => x.TenantId == tenantId && !x.IsDeleted)
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .ToListAsync(ct);
    }

    async Task<IEnumerable<ClientIndividualEntity>> IClientIndividualDataRepository.GetActiveAsync(int tenantId, CancellationToken ct)
    {
        return await _context.Set<ClientIndividualEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.FiscalData)
            .Where(x => x.TenantId == tenantId && x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .ToListAsync(ct);
    }

    async Task<ListPage<ClientIndividualEntity>> IClientIndividualDataRepository.GetPagedAsync(int tenantId, PagedFilter filter, CancellationToken ct)
    {
        var query = _context.Set<ClientIndividualEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.FiscalData)
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

    async Task<bool> IClientIndividualDataRepository.ExistsByClientIdAsync(int tenantId, int clientId, CancellationToken ct)
    {
        return await _context.Set<ClientIndividualEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.ClientId == clientId && !x.IsDeleted, ct);
    }

    async Task<bool> IClientIndividualDataRepository.ExistsByDocumentAsync(int tenantId, string documentType, string documentNumber, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(documentNumber))
            return false;

        return await _context.Set<ClientIndividualEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.DocumentType == documentType && x.DocumentNumber == documentNumber && !x.IsDeleted, ct);
    }

    async Task<bool> IClientIndividualDataRepository.ExistsByDocumentAsync(int tenantId, string documentType, string documentNumber, int excludeId, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(documentNumber))
            return false;

        return await _context.Set<ClientIndividualEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.TenantId == tenantId && x.DocumentType == documentType && x.DocumentNumber == documentNumber && x.Id != excludeId && !x.IsDeleted, ct);
    }

    async Task<bool> IClientIndividualDataRepository.AddAsync(ClientIndividualEntity entity, CancellationToken ct)
    {
        await _context.Set<ClientIndividualEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    async Task<bool> IClientIndividualDataRepository.UpdateAsync(ClientIndividualEntity entity, CancellationToken ct)
    {
        _context.Set<ClientIndividualEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    async Task<ClientCompanyEntity> IClientCompanyDataRepository.GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<ClientCompanyEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.FiscalData)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    async Task<ClientCompanyEntity> IClientCompanyDataRepository.GetByClientIdAsync(int clientId, CancellationToken ct)
    {
        return await _context.Set<ClientCompanyEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.FiscalData)
            .FirstOrDefaultAsync(x => x.ClientId == clientId && !x.IsDeleted, ct);
    }

    async Task<ClientCompanyEntity> IClientCompanyDataRepository.GetByTaxNumberAsync(string taxNumber, CancellationToken ct)
    {
        return await _context.Set<ClientCompanyEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.FiscalData)
            .FirstOrDefaultAsync(x => x.CompanyRegistration == taxNumber && !x.IsDeleted, ct);
    }

    async Task<IEnumerable<ClientCompanyEntity>> IClientCompanyDataRepository.GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<ClientCompanyEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.FiscalData)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.LegalName)
            .ToListAsync(ct);
    }

    async Task<IEnumerable<ClientCompanyEntity>> IClientCompanyDataRepository.GetActiveAsync(CancellationToken ct)
    {
        return await _context.Set<ClientCompanyEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.FiscalData)
            .Where(x => x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.LegalName)
            .ToListAsync(ct);
    }

    async Task<ListPage<ClientCompanyEntity>> IClientCompanyDataRepository.GetPagedAsync(PagedFilter filter, CancellationToken ct)
    {
        var query = _context.Set<ClientCompanyEntity>()
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.FiscalData)
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(x =>
                x.LegalName.Contains(filter.Search) ||
                x.TradeName.Contains(filter.Search) ||
                x.Email.Contains(filter.Search) ||
                x.CompanyRegistration.Contains(filter.Search));
        }

        var totalCount = await query.CountAsync(ct);
        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 10;

        var items = await query
            .OrderBy(x => x.LegalName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new ListPage<ClientCompanyEntity>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    async Task<bool> IClientCompanyDataRepository.ExistsByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<ClientCompanyEntity>()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    async Task<bool> IClientCompanyDataRepository.ExistsByClientIdAsync(int clientId, CancellationToken ct)
    {
        return await _context.Set<ClientCompanyEntity>()
            .AnyAsync(x => x.ClientId == clientId && !x.IsDeleted, ct);
    }

    async Task<bool> IClientCompanyDataRepository.ExistsByTaxNumberAsync(string taxNumber, CancellationToken ct)
    {
        return await _context.Set<ClientCompanyEntity>()
            .AnyAsync(x => x.CompanyRegistration == taxNumber && !x.IsDeleted, ct);
    }

    async Task<bool> IClientCompanyDataRepository.AddAsync(ClientCompanyEntity entity, CancellationToken ct)
    {
        await _context.Set<ClientCompanyEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    async Task<bool> IClientCompanyDataRepository.UpdateAsync(ClientCompanyEntity entity, CancellationToken ct)
    {
        _context.Set<ClientCompanyEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    async Task<IEnumerable<ClientIndividualFiscalDataEntity>> IClientIndividualFiscalDataDataRepository.GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<ClientIndividualFiscalDataEntity>()
            .AsNoTracking()
            .Include(x => x.ClientIndividual)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.TaxNumber)
            .ToListAsync(ct);
    }

    async Task<IEnumerable<ClientIndividualFiscalDataEntity>> IClientIndividualFiscalDataDataRepository.GetActiveAsync(CancellationToken ct)
    {
        return await _context.Set<ClientIndividualFiscalDataEntity>()
            .AsNoTracking()
            .Include(x => x.ClientIndividual)
            .Where(x => x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.TaxNumber)
            .ToListAsync(ct);
    }

    async Task<ClientIndividualFiscalDataEntity> IClientIndividualFiscalDataDataRepository.GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<ClientIndividualFiscalDataEntity>()
            .AsNoTracking()
            .Include(x => x.ClientIndividual)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    async Task<ClientIndividualFiscalDataEntity> IClientIndividualFiscalDataDataRepository.GetByClientIndividualIdAsync(int clientIndividualId, CancellationToken ct)
    {
        return await _context.Set<ClientIndividualFiscalDataEntity>()
            .AsNoTracking()
            .Include(x => x.ClientIndividual)
            .FirstOrDefaultAsync(x => x.ClientIndividualId == clientIndividualId && !x.IsDeleted, ct);
    }

    async Task<ClientIndividualFiscalDataEntity> IClientIndividualFiscalDataDataRepository.GetByTaxNumberAsync(string taxNumber, CancellationToken ct)
    {
        return await _context.Set<ClientIndividualFiscalDataEntity>()
            .AsNoTracking()
            .Include(x => x.ClientIndividual)
            .FirstOrDefaultAsync(x => x.TaxNumber == taxNumber && !x.IsDeleted, ct);
    }

    async Task<ListPage<ClientIndividualFiscalDataEntity>> IClientIndividualFiscalDataDataRepository.GetPagedAsync(PagedFilter filter, CancellationToken ct)
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

    async Task<bool> IClientIndividualFiscalDataDataRepository.ExistsByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<ClientIndividualFiscalDataEntity>()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    async Task<bool> IClientIndividualFiscalDataDataRepository.ExistsByClientIndividualIdAsync(int clientIndividualId, CancellationToken ct)
    {
        return await _context.Set<ClientIndividualFiscalDataEntity>()
            .AnyAsync(x => x.ClientIndividualId == clientIndividualId && !x.IsDeleted, ct);
    }

    async Task<bool> IClientIndividualFiscalDataDataRepository.ExistsByTaxNumberAsync(string taxNumber, int? excludeId, CancellationToken ct)
    {
        var query = _context.Set<ClientIndividualFiscalDataEntity>()
            .Where(x => x.TaxNumber == taxNumber && !x.IsDeleted);

        if (excludeId.HasValue)
            query = query.Where(x => x.Id != excludeId.Value);

        return await query.AnyAsync(ct);
    }

    async Task<bool> IClientIndividualFiscalDataDataRepository.AddAsync(ClientIndividualFiscalDataEntity entity, CancellationToken ct)
    {
        await _context.Set<ClientIndividualFiscalDataEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    async Task<bool> IClientIndividualFiscalDataDataRepository.UpdateAsync(ClientIndividualFiscalDataEntity entity, CancellationToken ct)
    {
        _context.Set<ClientIndividualFiscalDataEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    async Task<IEnumerable<ClientCompanyFiscalDataEntity>> IClientCompanyFiscalDataDataRepository.GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<ClientCompanyFiscalDataEntity>()
            .AsNoTracking()
            .Include(x => x.ClientCompany)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.TaxNumber)
            .ToListAsync(ct);
    }

    async Task<IEnumerable<ClientCompanyFiscalDataEntity>> IClientCompanyFiscalDataDataRepository.GetActiveAsync(CancellationToken ct)
    {
        return await _context.Set<ClientCompanyFiscalDataEntity>()
            .AsNoTracking()
            .Include(x => x.ClientCompany)
            .Where(x => x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.TaxNumber)
            .ToListAsync(ct);
    }

    async Task<ClientCompanyFiscalDataEntity> IClientCompanyFiscalDataDataRepository.GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<ClientCompanyFiscalDataEntity>()
            .AsNoTracking()
            .Include(x => x.ClientCompany)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    async Task<ClientCompanyFiscalDataEntity> IClientCompanyFiscalDataDataRepository.GetByClientCompanyIdAsync(int clientCompanyId, CancellationToken ct)
    {
        return await _context.Set<ClientCompanyFiscalDataEntity>()
            .AsNoTracking()
            .Include(x => x.ClientCompany)
            .FirstOrDefaultAsync(x => x.ClientCompanyId == clientCompanyId && !x.IsDeleted, ct);
    }

    async Task<ClientCompanyFiscalDataEntity> IClientCompanyFiscalDataDataRepository.GetByTaxNumberAsync(string taxNumber, CancellationToken ct)
    {
        return await _context.Set<ClientCompanyFiscalDataEntity>()
            .AsNoTracking()
            .Include(x => x.ClientCompany)
            .FirstOrDefaultAsync(x => x.TaxNumber == taxNumber && !x.IsDeleted, ct);
    }

    async Task<ListPage<ClientCompanyFiscalDataEntity>> IClientCompanyFiscalDataDataRepository.GetPagedAsync(PagedFilter filter, CancellationToken ct)
    {
        var query = _context.Set<ClientCompanyFiscalDataEntity>()
            .AsNoTracking()
            .Include(x => x.ClientCompany)
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

        return new ListPage<ClientCompanyFiscalDataEntity>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    async Task<bool> IClientCompanyFiscalDataDataRepository.ExistsByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Set<ClientCompanyFiscalDataEntity>()
            .AnyAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    async Task<bool> IClientCompanyFiscalDataDataRepository.ExistsByClientCompanyIdAsync(int clientCompanyId, CancellationToken ct)
    {
        return await _context.Set<ClientCompanyFiscalDataEntity>()
            .AnyAsync(x => x.ClientCompanyId == clientCompanyId && !x.IsDeleted, ct);
    }

    async Task<bool> IClientCompanyFiscalDataDataRepository.ExistsByTaxNumberAsync(string taxNumber, int? excludeId, CancellationToken ct)
    {
        var query = _context.Set<ClientCompanyFiscalDataEntity>()
            .Where(x => x.TaxNumber == taxNumber && !x.IsDeleted);

        if (excludeId.HasValue)
            query = query.Where(x => x.Id != excludeId.Value);

        return await query.AnyAsync(ct);
    }

    async Task<bool> IClientCompanyFiscalDataDataRepository.AddAsync(ClientCompanyFiscalDataEntity entity, CancellationToken ct)
    {
        await _context.Set<ClientCompanyFiscalDataEntity>().AddAsync(entity, ct);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    async Task<bool> IClientCompanyFiscalDataDataRepository.UpdateAsync(ClientCompanyFiscalDataEntity entity, CancellationToken ct)
    {
        _context.Set<ClientCompanyFiscalDataEntity>().Update(entity);
        return await _context.SaveChangesAsync(ct) > 0;
    }
}
