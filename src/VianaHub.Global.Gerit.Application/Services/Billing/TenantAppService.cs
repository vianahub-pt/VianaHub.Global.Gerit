using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Billing.Tenant;
using VianaHub.Global.Gerit.Application.Dtos.Response.Billing.Tenant;
using VianaHub.Global.Gerit.Application.Interfaces.Billing;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Helpers;
using VianaHub.Global.Gerit.Domain.Interfaces;
using VianaHub.Global.Gerit.Domain.Interfaces.Billing;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Application.Services.Billing;

public class TenantAppService : ITenantAppService
{
    private readonly ITenantDataRepository _repo;
    private readonly ITenantDomainService _domain;
    private readonly ICurrentUserService _currentUser;
    private readonly INotify _notify;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localization;

    public TenantAppService(
        ITenantDataRepository repo,
        ITenantDomainService domain,
        INotify notify,
        IMapper mapper,
        ICurrentUserService currentUser,
        ILocalizationService localization)
    {
        _repo = repo;
        _domain = domain;
        _notify = notify;
        _mapper = mapper;
        _currentUser = currentUser;
        _localization = localization;
    }

    public async Task<IEnumerable<TenantResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<TenantResponse>>(entities);
    }
    
    public async Task<TenantResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        return _mapper.Map<TenantResponse>(entity);
    }
    
    public async Task<ListPageResponse<TenantResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<TenantResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateTenantRequest request, CancellationToken ct)
    {
        var exists = await _repo.ExistsByNameAsync(request.Name, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Tenant.Create.ResourceAlreadyExists"), 400);
            return false;
        }

        var entity = new TenantEntity(request.Name, request.Consent, _currentUser.GetUserId());
        return await _domain.CreateAsync(entity, ct);
    }
    
    public async Task<bool> UpdateAsync(int id, UpdateTenantRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Tenant.Update.ResourceNotFound"), 410);
            return false;
        }

        entity.Update(request.Name, request.Consent, _currentUser.GetUserId());
        return await _domain.UpdateAsync(entity, ct);
    }
    
    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Tenant.Activate.ResourceNotFound"), 410);
            return false;
        }

        entity.Activate(_currentUser.GetUserId());
        return await _domain.ActivateAsync(entity, ct);
    }
    
    public async Task<bool> DeactivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Tenant.Deactivate.ResourceNotFound"), 410);
            return false;
        }

        entity.Deactivate(_currentUser.GetUserId());
        return await _domain.DeactivateAsync(entity, ct);
    }
    
    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Tenant.Delete.ResourceNotFound"), 410);
            return false;
        }

        entity.Delete(_currentUser.GetUserId());
        return await _domain.DeleteAsync(entity, ct);
    }
    
    public async Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct)
    {
        // Valida arquivo
        if (!ValidateFile(file))
            return false;

        // Lê itens do CSV
        var items = ReadCsvFile(file);
        if (items == null)
            return false;

        if (!items.Any())
        {
            _notify.Add(_localization.GetMessage("Application.Service.Tenant.BulkUpload.EmptyFile"), 400);
            return false;
        }

        // Processa cada item
        return await ProcessBulkItemsAsync(items, ct);
    }

    private bool ValidateFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Tenant.ValidateFile.InvalidFile"), 400);
            return false;
        }

        // Valida tamanho do arquivo
        if (!file.Length.IsValidCsvFileSize())
        {
            _notify.Add(_localization.GetMessage("Application.Service.Tenant.ValidateFile.IsValidCsvFileSize"), 400);
            return false;
        }

        // Valida nome do arquivo (previne path traversal)
        if (!file.FileName.IsSafeCsvFileName())
        {
            _notify.Add(_localization.GetMessage("Application.Service.Tenant.ValidateFile.IsSafeCsvFileName"), 400);
            return false;
        }

        // Valida extensão
        if (!file.FileName.HasValidCsvExtension())
        {
            _notify.Add(_localization.GetMessage("Application.Service.Tenant.ValidateFile.OnlyCsvAllowed"), 400);
            return false;
        }

        return true;
    }
    
    private List<BulkUploadTenantItem> ReadCsvFile(IFormFile file)
    {
        try
        {
            // Cria StreamReader com encoding UTF-8 forçado
            using var reader = file.OpenReadStream().CreateUtf8StreamReader();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";", // CSV usa ponto e vírgula como delimitador
                MissingFieldFound = null,
                HeaderValidated = null,
                TrimOptions = TrimOptions.Trim,
                BadDataFound = null // Ignora linhas mal formatadas ao invés de lançar exceção
            };

            using var csv = new CsvReader(reader, config);
            var records = new List<BulkUploadTenantItem>();

            csv.Read();
            csv.ReadHeader();

            int rowCount = 0;
            int maxRows = DomainExtensions.GetMaxCsvRows();

            while (csv.Read() && rowCount < maxRows)
            {
                try
                {
                    var record = csv.GetRecord<BulkUploadTenantItem>();
                    if (record != null)
                    {
                        // Sanitiza e normaliza campos
                        record.Name = record.Name?.SanitizeCsvInput().NormalizeUtf8();

                        // Valida se os campos não contêm conteúdo perigoso
                        if (!string.IsNullOrEmpty(record.Name) && !record.Name.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.Tenant.ReadCsvFile.Name.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        records.Add(record);
                    }
                    rowCount++;
                }
                catch (CsvHelperException ex)
                {
                    // Log linha com erro mas continua processamento
                    _notify.Add(_localization.GetMessage("Application.Service.Tenant.ReadCsvFile.CsvHelperException", rowCount + 2), 400);
                    rowCount++;
                    continue;
                }
            }

            if (rowCount >= maxRows)
            {
                _notify.Add(_localization.GetMessage("Application.Service.Tenant.ReadCsvFile.MaxRows", maxRows), 400);
                return null;
            }

            return records;
        }
        catch (Exception ex)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Tenant.ReadCsvFile.Exception"), 400);
            return null;
        }
    }
    
    private async Task<bool> ProcessBulkItemsAsync(List<BulkUploadTenantItem> items, CancellationToken ct)
    {
        var hasErrors = false;

        foreach (var item in items)
        {
            // Valida campos obrigatórios
            if (!ValidateBulkItem(item))
            {
                hasErrors = true;
                continue;
            }

            // Verifica duplicidade
            var exists = await _repo.ExistsByNameAsync(item.Name, ct);
            if (exists)
            {
                _notify.Add(_localization.GetMessage("Application.Service.Tenant.ProcessBulkItems.ExistsBylName", item.Name), 400);
                hasErrors = true;
                continue;
            }

            // Cria a entidade
            var entity = new TenantEntity(item.Name, item.Consent, _currentUser.GetUserId());

            // Tenta criar no domínio
            var success = await _domain.CreateAsync(entity, ct);

            if (!success)
            {
                _notify.Add(_localization.GetMessage("Application.Service.Tenant.ProcessBulkItems.FailedToCreate", item.Name), 400);
                hasErrors = true;
            }
        }

        return !hasErrors;
    }
    
    private bool ValidateBulkItem(BulkUploadTenantItem item)
    {
        if (string.IsNullOrWhiteSpace(item.Name))
        {
            _notify.Add(_localization.GetMessage("Application.Service.Tenant.ValidateBulkItem.Name", item.Name), 400);
            return false;
        }

        return true;
    }
}
