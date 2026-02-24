using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Globalization;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionContact;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionContact;
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
/// Serviço de aplicação para InterventionContact
/// </summary>
public class InterventionContactAppService : IInterventionContactAppService
{
    private readonly IInterventionContactDataRepository _repo;
    private readonly IInterventionContactDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;
    private readonly IFileValidationService _fileValidation;
    private readonly ILogger<InterventionContactAppService> _logger;

    public InterventionContactAppService(
        IInterventionContactDataRepository repo,
        IInterventionContactDomainService domain,
        IMapper mapper,
        INotify notify,
        ILocalizationService localization,
        ICurrentUserService currentUser,
        IFileValidationService fileValidation,
        ILogger<InterventionContactAppService> logger)
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

    public async Task<IEnumerable<InterventionContactResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<InterventionContactResponse>>(entities);
    }

    public async Task<InterventionContactResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionContact.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<InterventionContactResponse>(entity);
    }

    public async Task<ListPageResponse<InterventionContactResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<InterventionContactResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateInterventionContactRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var userId = _currentUser.GetUserId();

        // Validar unicidade: Email único por Intervenção
        var exists = await _repo.ExistsByInterventionAndEmailAsync(request.InterventionId, request.Email, null, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionContact.Create.ResourceAlreadyExists"), 409);
            return false;
        }

        var entity = new InterventionContactEntity(
            tenantId,
            request.InterventionId,
            request.Name,
            request.Email,
            request.Phone,
            request.IsPrimary,
            userId
        );

        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateInterventionContactRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionContact.Update.ResourceNotFound"), 410);
            return false;
        }

        // Validar unicidade: Email único por Intervenção (excluindo o próprio registro)
        var exists = await _repo.ExistsByInterventionAndEmailAsync(entity.InterventionId, request.Email, id, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionContact.Update.ResourceAlreadyExists"), 409);
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
            _notify.Add(_localization.GetMessage("Application.Service.InterventionContact.Activate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.InterventionContact.Deactivate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.InterventionContact.Delete.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.InterventionContact.BulkUpload.EmptyFile"), 400);
            return false;
        }

        // Processa cada item
        return await ProcessBulkItemsAsync(items, ct);
    }

    private List<BulkUploadInterventionContactItem> ReadCsvFile(IFormFile file)
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
            var records = new List<BulkUploadInterventionContactItem>();

            csv.Read();
            csv.ReadHeader();

            int rowCount = 0;
            int maxRows = DomainExtensions.GetMaxCsvRows();

            while (csv.Read() && rowCount < maxRows)
            {
                try
                {
                    var record = csv.GetRecord<BulkUploadInterventionContactItem>();
                    if (record != null)
                    {
                        // Sanitiza e normaliza campos
                        record.Name = record.Name?.SanitizeCsvInput().NormalizeUtf8();
                        record.Email = record.Email?.SanitizeCsvInput().NormalizeUtf8();
                        record.Phone = record.Phone?.SanitizeCsvInput().NormalizeUtf8();

                        // Valida se os campos não contêm conteúdo perigoso
                        if (!string.IsNullOrEmpty(record.Name) && !record.Name.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.InterventionContact.ReadCsvFile.Name.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        if (!string.IsNullOrEmpty(record.Email) && !record.Email.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.InterventionContact.ReadCsvFile.Email.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        if (!string.IsNullOrEmpty(record.Phone) && !record.Phone.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.InterventionContact.ReadCsvFile.Phone.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        records.Add(record);
                    }
                    rowCount++;
                }
                catch (CsvHelperException ex)
                {
                    // Log linha com erro mas continua processamento
                    _logger.LogWarning(ex, "Erro ao processar linha {RowNumber} do CSV de InterventionContacts", rowCount + 2);
                    _notify.Add(_localization.GetMessage("Application.Service.InterventionContact.ReadCsvFile.CsvHelperException", rowCount + 2), 400);
                    rowCount++;
                    continue;
                }
            }

            if (rowCount >= maxRows)
            {
                _notify.Add(_localization.GetMessage("Application.Service.InterventionContact.ReadCsvFile.MaxRows", maxRows), 400);
                return null;
            }

            return records;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao ler arquivo CSV de InterventionContacts: {Message}", ex.Message);
            _notify.Add(_localization.GetMessage("Application.Service.InterventionContact.ReadCsvFile.Exception"), 400);
            return null;
        }
    }

    private async Task<bool> ProcessBulkItemsAsync(List<BulkUploadInterventionContactItem> items, CancellationToken ct)
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
            var exists = await _repo.ExistsByInterventionAndEmailAsync(item.InterventionId, item.Email, null, ct);
            if (exists)
            {
                _notify.Add(_localization.GetMessage("Application.Service.InterventionContact.ProcessBulkItems.ExistsByInterventionAndEmail", item.Email, item.InterventionId), 400);
                hasErrors = true;
                continue;
            }

            var entity = new InterventionContactEntity(
                tenantId,
                item.InterventionId,
                item.Name,
                item.Email,
                item.Phone,
                item.IsPrimary,
                userId
            );

            var created = await _domain.CreateAsync(entity, ct);
            if (!created)
            {
                _notify.Add(_localization.GetMessage("Application.Service.InterventionContact.ProcessBulkItems.FailedToCreate", item.Name), 400);
                hasErrors = true;
            }
        }

        return !hasErrors;
    }

    private bool ValidateBulkItem(BulkUploadInterventionContactItem item)
    {
        if (item.InterventionId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionContact.ValidateBulkItem.InterventionTeamId"), 400);
            return false;
        }

        if (string.IsNullOrWhiteSpace(item.Name))
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionContact.ValidateBulkItem.Name"), 400);
            return false;
        }

        if (string.IsNullOrWhiteSpace(item.Email))
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionContact.ValidateBulkItem.Email"), 400);
            return false;
        }

        return true;
    }
}
