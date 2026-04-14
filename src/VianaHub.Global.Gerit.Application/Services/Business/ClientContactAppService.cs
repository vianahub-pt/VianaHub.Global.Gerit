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
/// Servi�o de aplica��o para ClientContact
/// </summary>
public class ClientContactAppService : IClientContactAppService
{
    private readonly IClientContactDataRepository _repo;
    private readonly IClientRepository _clientRepository;
    private readonly IClientContactDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;
    private readonly IFileValidationService _fileValidation;
    private readonly ILogger<ClientContactAppService> _logger;

    public ClientContactAppService(
        IClientContactDataRepository repo,
        IClientRepository clientRepository,
        IClientContactDomainService domain,
        IMapper mapper,
        INotify notify,
        ILocalizationService localization,
        ICurrentUserService currentUser,
        IFileValidationService fileValidation,
        ILogger<ClientContactAppService> logger)
    {
        _repo = repo;
        _clientRepository = clientRepository;
        _domain = domain;
        _mapper = mapper;
        _notify = notify;
        _localization = localization;
        _currentUser = currentUser;
        _fileValidation = fileValidation;
        _logger = logger;
    }

    public async Task<IEnumerable<ClientContactResponse>> GetAllAsync(int clientId, CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(clientId, ct);
        return _mapper.Map<IEnumerable<ClientContactResponse>>(entities);
    }

    public async Task<ClientContactResponse> GetByIdAsync(int clientId, int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(clientId, id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<ClientContactResponse>(entity);
    }

    public async Task<ListPageResponse<ClientContactResponse>> GetPagedAsync(int clientId, PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.IsActive, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(clientId, filter, ct);
        return _mapper.Map<ListPageResponse<ClientContactResponse>>(paged);
    }

    public async Task<bool> CreateAsync(int clientId, CreateClientContactRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var userId = _currentUser.GetUserId();
        var client = await _clientRepository.GetAggregateForUpdateAsync(tenantId, clientId, ct);
        if (client == null || client.IsDeleted || !client.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.Create.ResourceNotFound"), 410);
            return false;
        }

        // Validar unicidade: Email �nico por Cliente
        var exists = await _repo.ExistsByClientAndEmailAsync(clientId, request.Email, null, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.Create.ResourceAlreadyExists"), 409);
            return false;
        }

        var entity = new ClientContactEntity(
            tenantId,
            clientId,
            request.Name,
            request.Email,
            request.Phone,
            request.IsPrimary,
            userId
        );

        client.AddContact(entity, userId);
        return await _clientRepository.UpdateAsync(client, ct);
    }

    public async Task<bool> UpdateAsync(int clientId, int id, UpdateClientContactRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(clientId, id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.Update.ResourceNotFound"), 410);
            return false;
        }

        var client = await _clientRepository.GetAggregateForUpdateAsync(_currentUser.GetTenantId(), entity.ClientId, ct);
        if (client == null || client.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.Update.ResourceNotFound"), 410);
            return false;
        }

        var trackedContact = client.FindContact(id);
        if (trackedContact == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.Update.ResourceNotFound"), 410);
            return false;
        }

        // Validar unicidade: Email �nico por Cliente (excluindo o pr�prio registro)
        var exists = await _repo.ExistsByClientAndEmailAsync(entity.ClientId, request.Email, id, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.Update.ResourceAlreadyExists"), 409);
            return false;
        }

        trackedContact.Update(request.Name, request.Email, request.Phone, request.IsPrimary, _currentUser.GetUserId());
        if (request.IsPrimary)
        {
            client.EnsureSinglePrimaryContact(id, _currentUser.GetUserId());
        }
        else if (trackedContact.IsPrimary)
        {
            trackedContact.RemoveAsPrimary(_currentUser.GetUserId());
        }

        return await _clientRepository.UpdateAsync(client, ct);
    }

    public async Task<bool> ActivateAsync(int clientId, int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(clientId, id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.Activate.ResourceNotFound"), 410);
            return false;
        }

        var client = await _clientRepository.GetAggregateForUpdateAsync(_currentUser.GetTenantId(), entity.ClientId, ct);
        var trackedContact = client?.FindContact(id);
        if (trackedContact == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.Activate.ResourceNotFound"), 410);
            return false;
        }

        trackedContact.Activate(_currentUser.GetUserId());
        return await _clientRepository.UpdateAsync(client!, ct);
    }

    public async Task<bool> DeactivateAsync(int clientId, int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(clientId, id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.Deactivate.ResourceNotFound"), 410);
            return false;
        }

        var client = await _clientRepository.GetAggregateForUpdateAsync(_currentUser.GetTenantId(), entity.ClientId, ct);
        var trackedContact = client?.FindContact(id);
        if (trackedContact == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.Deactivate.ResourceNotFound"), 410);
            return false;
        }

        trackedContact.Deactivate(_currentUser.GetUserId());
        return await _clientRepository.UpdateAsync(client!, ct);
    }

    public async Task<bool> DeleteAsync(int clientId, int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(clientId, id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.Delete.ResourceNotFound"), 410);
            return false;
        }

        var client = await _clientRepository.GetAggregateForUpdateAsync(_currentUser.GetTenantId(), entity.ClientId, ct);
        var trackedContact = client?.FindContact(id);
        if (trackedContact == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.Delete.ResourceNotFound"), 410);
            return false;
        }

        trackedContact.Delete(_currentUser.GetUserId());
        return await _clientRepository.UpdateAsync(client!, ct);
    }

    public async Task<bool> BulkUploadAsync(int clientId, IFormFile file, CancellationToken ct)
    {
        // Valida arquivo usando servi�o centralizado
        if (!_fileValidation.ValidateFile(file))
            return false;

        // L� itens do CSV
        var items = ReadCsvFile(file);
        if (items == null)
            return false;

        if (!items.Any())
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientContact.BulkUpload.EmptyFile"), 400);
            return false;
        }

        // Processa cada item
        return await ProcessBulkItemsAsync(clientId, items, ct);
    }

    private List<BulkUploadClientContactItem> ReadCsvFile(IFormFile file)
    {
        try
        {
            // Cria StreamReader com encoding UTF-8 for�ado
            using var reader = file.OpenReadStream().CreateUtf8StreamReader();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";", // CSV usa ponto e v�rgula como delimitador
                MissingFieldFound = null,
                HeaderValidated = null,
                TrimOptions = TrimOptions.Trim,
                BadDataFound = null // Ignora linhas mal formatadas ao inv�s de lan�ar exce��o
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

                        // Valida se os campos n�o cont�m conte�do perigoso
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

    private async Task<bool> ProcessBulkItemsAsync(int clientId, List<BulkUploadClientContactItem> items, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var userId = _currentUser.GetUserId();
        var hasErrors = false;

        foreach (var item in items)
        {
            // Valida��o b�sica
            if (!ValidateBulkItem(item))
            {
                hasErrors = true;
                continue;
            }

            // Verificar se j� existe
            var exists = await _repo.ExistsByClientAndEmailAsync(clientId, item.Email, null, ct);
            if (exists)
            {
                _notify.Add(_localization.GetMessage("Application.Service.ClientContact.ProcessBulkItems.ExistsByClientAndEmail", item.Email, clientId), 400);
                hasErrors = true;
                continue;
            }

            var entity = new ClientContactEntity(
                tenantId,
                clientId,
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

