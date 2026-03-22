using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Globalization;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientContact;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientContact;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Application.Interfaces.Common;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Helpers;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Application.Services.Business;

/// <summary>
/// Serviço de aplicação para ClientContact
/// </summary>
public class ClientContactAppService : IClientContactAppService
{
    private readonly IClientContactDataRepository _repo;
    private readonly IClientContactDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;
    private readonly IFileValidationService _fileValidation;
    private readonly ILogger<ClientContactAppService> _logger;

    public ClientContactAppService(
        IClientContactDataRepository repo,
        IClientContactDomainService domain,
        IMapper mapper,
        INotify notify,
        ILocalizationService localization,
        ICurrentUserService currentUser,
        IFileValidationService fileValidation,
        ILogger<ClientContactAppService> logger)
    {
        _repo = repo;
        _domain = domain;
        _mapper = mapper;
        _notify = notify;
        _localization = localization;
        _currentUser = currentUser;
        _fileValidation = fileValidation;
        _logger = logger;
    }

    public async Task<IEnumerable<ClientContactResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<ClientContactResponse>>(entities);
    }

    public async Task<ClientContactResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<ClientContactResponse>(entity);
    }

    public async Task<ListPageResponse<ClientContactResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.IsActive, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<ClientContactResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateClientContactRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var userId = _currentUser.GetUserId();

        // Validar unicidade: Email único por Cliente
        var exists = await _repo.ExistsByClientAndEmailAsync(request.ClientId, request.Email, null, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.Create.ResourceAlreadyExists"), 409);
            return false;
        }

        var entity = new ClientContactEntity(
            tenantId,
            request.ClientId,
            request.Name,
            request.Email,
            request.Phone,
            request.IsPrimary,
            userId
        );

        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateClientContactRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.Update.ResourceNotFound"), 410);
            return false;
        }

        // Validar unicidade: Email único por Cliente (excluindo o próprio registro)
        var exists = await _repo.ExistsByClientAndEmailAsync(entity.ClientId, request.Email, id, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.Update.ResourceAlreadyExists"), 409);
            return false;
        }

        entity.Update(request.Name, request.Email, request.Phone, request.IsPrimary, _currentUser.GetUserId());
        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.Activate.ResourceNotFound"), 410);
            return false;
        }

        entity.Activate(_currentUser.GetUserId());
        return await _domain.ActivateAsync(entity, ct);
    }

    public async Task<bool> DeactivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.Deactivate.ResourceNotFound"), 410);
            return false;
        }

        entity.Deactivate(_currentUser.GetUserId());
        return await _domain.DeactivateAsync(entity, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.Delete.ResourceNotFound"), 410);
            return false;
        }

        entity.Delete(_currentUser.GetUserId());
        return await _domain.DeleteAsync(entity, ct);
    }

    public async Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct)
    {
        // Valida arquivo usando serviço centralizado
        if (!_fileValidation.ValidateFile(file))
            return false;

        // Lê itens do CSV
        var items = ReadCsvFile(file);
        if (items == null)
            return false;

        if (!items.Any())
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.BulkUpload.EmptyFile"), 400);
            return false;
        }

        // Processa cada item
        return await ProcessBulkItemsAsync(items, ct);
    }

    private List<BulkUploadClientContactItem> ReadCsvFile(IFormFile file)
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
            var records = new List<BulkUploadClientContactItem>();

            csv.Read();
            csv.ReadHeader();

            int rowCount = 0;
            int maxRows = DomainExtensions.GetMaxCsvRows();

            while (csv.Read() && rowCount < maxRows)
            {
                try
                {
                    var record = csv.GetRecord<BulkUploadClientContactItem>();
                    if (record != null)
                    {
                        // Sanitiza e normaliza campos
                        record.Name = record.Name?.SanitizeCsvInput().NormalizeUtf8();
                        record.Email = record.Email?.SanitizeCsvInput().NormalizeUtf8();
                        record.Phone = record.Phone?.SanitizeCsvInput().NormalizeUtf8();

                        // Valida se os campos não contêm conteúdo perigoso
                        if (!string.IsNullOrEmpty(record.Name) && !record.Name.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.ReadCsvFile.Name.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        if (!string.IsNullOrEmpty(record.Email) && !record.Email.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.ReadCsvFile.Email.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        if (!string.IsNullOrEmpty(record.Phone) && !record.Phone.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.ReadCsvFile.Phone.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        records.Add(record);
                    }
                    rowCount++;
                }
                catch (CsvHelperException ex)
                {
                    // Log linha com erro mas continua processamento
                    _logger.LogWarning(ex, "Erro ao processar linha {RowNumber} do CSV de ClientContacts", rowCount + 2);
                    _notify.Add(_localization.GetMessage("Application.Service.ClientContact.ReadCsvFile.CsvHelperException", rowCount + 2), 400);
                    rowCount++;
                    continue;
                }
            }

            if (rowCount >= maxRows)
            {
                _notify.Add(_localization.GetMessage("Application.Service.ClientContact.ReadCsvFile.MaxRows", maxRows), 400);
                return null;
            }

            return records;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao ler arquivo CSV de ClientContacts: {Message}", ex.Message);
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.ReadCsvFile.Exception"), 400);
            return null;
        }
    }

    private async Task<bool> ProcessBulkItemsAsync(List<BulkUploadClientContactItem> items, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var userId = _currentUser.GetUserId();
        var hasErrors = false;

        foreach (var item in items)
        {
            // Validação básica
            if (!ValidateBulkItem(item))
            {
                hasErrors = true;
                continue;
            }

            // Verificar se já existe
            var exists = await _repo.ExistsByClientAndEmailAsync(item.ClientId, item.Email, null, ct);
            if (exists)
            {
                _notify.Add(_localization.GetMessage("Application.Service.ClientContact.ProcessBulkItems.ExistsByClientAndEmail", item.Email, item.ClientId), 400);
                hasErrors = true;
                continue;
            }

            var entity = new ClientContactEntity(
                tenantId,
                item.ClientId,
                item.Name,
                item.Email,
                item.Phone,
                item.IsPrimary,
                userId
            );

            var created = await _domain.CreateAsync(entity, ct);
            if (!created)
            {
                _notify.Add(_localization.GetMessage("Application.Service.ClientContact.ProcessBulkItems.FailedToCreate", item.Name), 400);
                hasErrors = true;
            }
        }

        return !hasErrors;
    }

    private bool ValidateBulkItem(BulkUploadClientContactItem item)
    {
        if (item.ClientId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.ValidateBulkItem.ClientId"), 400);
            return false;
        }

        if (string.IsNullOrWhiteSpace(item.Name))
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.ValidateBulkItem.Name"), 400);
            return false;
        }

        if (string.IsNullOrWhiteSpace(item.Email))
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.ValidateBulkItem.Email"), 400);
            return false;
        }

        return true;
    }
}
