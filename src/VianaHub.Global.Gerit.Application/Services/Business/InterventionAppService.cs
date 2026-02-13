using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Globalization;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Intervention;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.Intervention;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Application.Interfaces.Common;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Helpers;
using VianaHub.Global.Gerit.Domain.Interfaces;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Application.Services.Business;

/// <summary>
/// Serviço de aplicação para Intervention
/// </summary>
public class InterventionAppService : IInterventionAppService
{
    private readonly IInterventionDataRepository _repo;
    private readonly IInterventionDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;
    private readonly IFileValidationService _fileValidation;
    private readonly ILogger<InterventionAppService> _logger;

    public InterventionAppService(
        IInterventionDataRepository repo,
        IInterventionDomainService domain,
        IMapper mapper,
        INotify notify,
        ILocalizationService localization,
        ICurrentUserService currentUser,
        IFileValidationService fileValidation,
        ILogger<InterventionAppService> logger)
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

    public async Task<IEnumerable<InterventionResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<InterventionResponse>>(entities);
    }

    public async Task<InterventionResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Intervention.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<InterventionResponse>(entity);
    }

    public async Task<ListPageResponse<InterventionResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<InterventionResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateInterventionRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var userId = _currentUser.GetUserId();

        // Validar unicidade: Título único por Tenant
        var exists = await _repo.ExistsByTenantAndTitleAsync(tenantId, request.Title, null, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Intervention.Create.ResourceAlreadyExists"), 409);
            return false;
        }

        var entity = new InterventionEntity(
            tenantId,
            request.ClientId,
            request.TeamMemberId,
            request.VehicleId,
            request.InterventionStatusId,
            request.Title,
            request.Description,
            request.StartDateTime,
            request.EstimatedValue,
            userId
        );

        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateInterventionRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Intervention.Update.ResourceNotFound"), 410);
            return false;
        }

        var tenantId = _currentUser.GetTenantId();

        // Validar unicidade: Título único por Tenant (excluindo o próprio registro)
        var exists = await _repo.ExistsByTenantAndTitleAsync(tenantId, request.Title, id, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Intervention.Create.ResourceAlreadyExists"), 409);
            return false;
        }

        entity.UpdateDetails(
            request.ClientId,
            request.TeamMemberId,
            request.VehicleId,
            request.InterventionStatusId,
            request.Title,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            request.EstimatedValue,
            request.RealValue,
            _currentUser.GetUserId()
        );

        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Intervention.Activate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.Intervention.Deactivate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.Intervention.Delete.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.Intervention.BulkUpload.EmptyFile"), 400);
            return false;
        }

        // Processa cada item
        return await ProcessBulkItemsAsync(items, ct);
    }

    private List<BulkUploadInterventionItem> ReadCsvFile(IFormFile file)
    {
        try
        {
            using var reader = file.OpenReadStream().CreateUtf8StreamReader();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";",
                MissingFieldFound = null,
                HeaderValidated = null,
                TrimOptions = TrimOptions.Trim,
                BadDataFound = null
            };

            using var csv = new CsvReader(reader, config);
            var records = new List<BulkUploadInterventionItem>();

            csv.Read();
            csv.ReadHeader();

            int rowCount = 0;
            int maxRows = DomainExtensions.GetMaxCsvRows();

            while (csv.Read() && rowCount < maxRows)
            {
                try
                {
                    var record = csv.GetRecord<BulkUploadInterventionItem>();
                    if (record != null)
                    {
                        record.Title = record.Title?.SanitizeCsvInput().NormalizeUtf8();
                        record.Description = record.Description?.SanitizeCsvInput().NormalizeUtf8();

                        if (!string.IsNullOrEmpty(record.Title) && !record.Title.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.Intervention.ReadCsvFile.Title.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        if (!string.IsNullOrEmpty(record.Description) && !record.Description.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.Intervention.ReadCsvFile.Description.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        records.Add(record);
                    }
                    rowCount++;
                }
                catch (CsvHelperException ex)
                {
                    _logger.LogWarning(ex, "Erro ao processar linha {RowNumber} do CSV de Interventions", rowCount + 2);
                    _notify.Add(_localization.GetMessage("Application.Service.Intervention.ReadCsvFile.CsvHelperException", rowCount + 2), 400);
                    rowCount++;
                    continue;
                }
            }

            if (rowCount >= maxRows)
            {
                _notify.Add(_localization.GetMessage("Application.Service.Intervention.ReadCsvFile.MaxRows", maxRows), 400);
                return null;
            }

            return records;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao ler arquivo CSV de Interventions: {Message}", ex.Message);
            _notify.Add(_localization.GetMessage("Application.Service.Intervention.ReadCsvFile.Exception"), 400);
            return null;
        }
    }

    private async Task<bool> ProcessBulkItemsAsync(List<BulkUploadInterventionItem> items, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var userId = _currentUser.GetUserId();
        var hasErrors = false;

        foreach (var item in items)
        {
            if (!ValidateBulkItem(item))
            {
                hasErrors = true;
                continue;
            }

            var exists = await _repo.ExistsByTenantAndTitleAsync(tenantId, item.Title, null, ct);
            if (exists)
            {
                _notify.Add(_localization.GetMessage("Application.Service.Intervention.ProcessBulkItems.ExistsByTitle", item.Title), 400);
                hasErrors = true;
                continue;
            }

            var entity = new InterventionEntity(
                tenantId,
                item.ClientId,
                item.TeamMemberId,
                item.VehicleId,
                item.InterventionStatusId,
                item.Title,
                item.Description,
                item.StartDateTime,
                item.EstimatedValue,
                userId
            );

            var created = await _domain.CreateAsync(entity, ct);
            if (!created)
            {
                _notify.Add(_localization.GetMessage("Application.Service.Intervention.ProcessBulkItems.FailedToCreate", item.Title), 400);
                hasErrors = true;
            }
        }

        return !hasErrors;
    }

    private bool ValidateBulkItem(BulkUploadInterventionItem item)
    {
        if (item.ClientId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Intervention.ValidateBulkItem.ClientId"), 400);
            return false;
        }

        if (item.TeamMemberId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Intervention.ValidateBulkItem.TeamMemberId"), 400);
            return false;
        }

        if (item.VehicleId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Intervention.ValidateBulkItem.VehicleId"), 400);
            return false;
        }

        if (item.InterventionStatusId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Intervention.ValidateBulkItem.InterventionStatusId"), 400);
            return false;
        }

        if (string.IsNullOrWhiteSpace(item.Title))
        {
            _notify.Add(_localization.GetMessage("Application.Service.Intervention.ValidateBulkItem.Title"), 400);
            return false;
        }

        if (string.IsNullOrWhiteSpace(item.Description))
        {
            _notify.Add(_localization.GetMessage("Application.Service.Intervention.ValidateBulkItem.Description"), 400);
            return false;
        }

        if (item.StartDateTime == default)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Intervention.ValidateBulkItem.StartDateTime"), 400);
            return false;
        }

        if (item.EstimatedValue < 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Intervention.ValidateBulkItem.EstimatedValue"), 400);
            return false;
        }

        return true;
    }
}

