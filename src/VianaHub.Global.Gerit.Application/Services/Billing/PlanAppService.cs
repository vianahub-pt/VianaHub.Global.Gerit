using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Globalization;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Billing.Plan;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Function;
using VianaHub.Global.Gerit.Application.Dtos.Response.Billing.Plan;
using VianaHub.Global.Gerit.Application.Interfaces.Billing;
using VianaHub.Global.Gerit.Application.Interfaces.Common;
using VianaHub.Global.Gerit.Application.Services.Business;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Helpers;
using VianaHub.Global.Gerit.Domain.Interfaces;
using VianaHub.Global.Gerit.Domain.Interfaces.Billing;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Application.Services.Billing;

public class PlanAppService : IPlanAppService
{
    private readonly IPlanDataRepository _repo;
    private readonly IPlanDomainService _domain;
    private readonly ICurrentUserService _currentUser;
    private readonly INotify _notify;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localization;
    private readonly ILogger<PlanAppService> _logger;
    private readonly IFileValidationService _fileValidation;

    public PlanAppService(
        IPlanDataRepository repo,
        IPlanDomainService domain,
        INotify notify,
        IMapper mapper,
        ICurrentUserService currentUser,
        ILocalizationService localization,
        ILogger<PlanAppService> logger,
        IFileValidationService fileValidation)
    {
        _repo = repo;
        _domain = domain;
        _notify = notify;
        _mapper = mapper;
        _currentUser = currentUser;
        _localization = localization;
        _logger = logger;
        _fileValidation = fileValidation;
    }

    public async Task<IEnumerable<PlanResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<PlanResponse>>(entities);
    }

    public async Task<PlanResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        return _mapper.Map<PlanResponse>(entity);
    }

    public async Task<ListPageResponse<PlanResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<PlanResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreatePlanRequest request, CancellationToken ct)
    {
        var exists = await _repo.ExistsByNameAsync(request.Name, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Plan.Create.ResourceAlreadyExists"), 400);
            return false;
        }

        var entity = new PlanEntity(
            request.Name,
            request.Description,
            request.PricePerHour,
            request.PricePerDay,
            request.PricePerMonth,
            request.PricePerYear,
            request.Currency ?? "USD",
            request.MaxUsers,
            request.MaxPhotosPerInterventions,
            _currentUser.GetUserId());

        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdatePlanRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Plan.Update.ResourceNotFound"), 410);
            return false;
        }

        entity.Update(
            request.Name,
            request.Description,
            request.PricePerHour,
            request.PricePerDay,
            request.PricePerMonth,
            request.PricePerYear,
            request.Currency ?? "USD",
            request.MaxUsers,
            request.MaxPhotosPerInterventions,
            _currentUser.GetUserId());

        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Plan.Activate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.Plan.Deactivate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.Plan.Delete.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.Plan.BulkUpload.EmptyFile"), 400);
            return false;
        }

        // Processa cada item
        return await ProcessBulkItemsAsync(items, ct);
    }

    private List<BulkUploadPlanItem> ReadCsvFile(IFormFile file)
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
            var records = new List<BulkUploadPlanItem>();

            csv.Read();
            csv.ReadHeader();

            int rowCount = 0;
            int maxRows = DomainExtensions.GetMaxCsvRows();

            while (csv.Read() && rowCount < maxRows)
            {
                try
                {
                    var record = csv.GetRecord<BulkUploadPlanItem>();
                    if (record != null)
                    {
                        // Sanitiza e normaliza campos
                        record.Name = record.Name?.SanitizeCsvInput().NormalizeUtf8();
                        record.Description = record.Description?.SanitizeCsvInput().NormalizeUtf8();
                        record.PricePerHour = record.PricePerHour;
                        record.PricePerDay = record.PricePerDay;
                        record.PricePerMonth = record.PricePerMonth;
                        record.PricePerYear = record.PricePerYear;
                        record.Currency = record.Currency?.SanitizeCsvInput().NormalizeUtf8();
                        record.MaxUsers = record.MaxUsers;
                        record.MaxPhotosPerInterventions = record.MaxPhotosPerInterventions;

                        // Valida se os campos não contêm conteúdo perigoso
                        if (!string.IsNullOrEmpty(record.Name) && !record.Name.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.Plan.ReadCsvFile.Name.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        if (!string.IsNullOrEmpty(record.Description) && !record.Description.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.Plan.ReadCsvFile.Description.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        records.Add(record);
                    }
                    rowCount++;
                }
                catch (CsvHelperException ex)
                {
                    // Log linha com erro mas continua processamento
                    _logger.LogWarning(ex, "Erro ao processar linha {RowNumber} do CSV de Plans", rowCount + 2);
                    _notify.Add(_localization.GetMessage("Application.Service.Plan.ReadCsvFile.CsvHelperException", rowCount + 2), 400);
                    rowCount++;
                    continue;
                }
            }

            if (rowCount >= maxRows)
            {
                _notify.Add(_localization.GetMessage("Application.Service.Plan.ReadCsvFile.MaxRows", maxRows), 400);
                return null;
            }

            return records;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao ler arquivo CSV de Plans: {Message}", ex.Message);
            _notify.Add(_localization.GetMessage("Application.Service.Plan.ReadCsvFile.Exception"), 400);
            return null;
        }
    }

    private async Task<bool> ProcessBulkItemsAsync(List<BulkUploadPlanItem> items, CancellationToken ct)
    {
        var hasErrors = false;
        var tenantId = _currentUser.GetTenantId();

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
                _notify.Add(_localization.GetMessage("Application.Service.Plan.ProcessBulkItems.ExistsByName", item.Name), 400);
                hasErrors = true;
                continue;
            }

            // Cria a entidade
            var entity = new PlanEntity(item.Name, item.Description, item.PricePerHour, item.PricePerDay, item.PricePerMonth, item.PricePerYear, item.Currency, item.MaxUsers, item.MaxPhotosPerInterventions, _currentUser.GetUserId());

            // Tenta criar no domínio
            var success = await _domain.CreateAsync(entity, ct);

            if (!success)
            {
                _notify.Add(_localization.GetMessage("Application.Service.Plan.ProcessBulkItems.FailedToCreate", item.Name), 400);
                hasErrors = true;
            }
        }

        return !hasErrors;
    }

    private bool ValidateBulkItem(BulkUploadPlanItem item)
    {
        if (string.IsNullOrWhiteSpace(item.Name))
        {
            _notify.Add(_localization.GetMessage("Application.Service.Plan.ValidateBulkItem.Name", item.Name), 400);
            return false;
        }

        return true;
    }
}
