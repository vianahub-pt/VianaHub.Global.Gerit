using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Globalization;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Billing.Subscription;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Function;
using VianaHub.Global.Gerit.Application.Dtos.Response.Billing.Subscription;
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

public class SubscriptionAppService : ISubscriptionAppService
{
    private readonly ISubscriptionDataRepository _repo;
    private readonly ISubscriptionDomainService _domain;
    private readonly ICurrentUserService _currentUser;
    private readonly INotify _notify;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localization;
    private readonly ILogger<FunctionAppService> _logger;
    private readonly IFileValidationService _fileValidation;

    public SubscriptionAppService(
        ISubscriptionDataRepository repo,
        ISubscriptionDomainService domain,
        INotify notify,
        IMapper mapper,
        ICurrentUserService currentUser,
        ILocalizationService localization,
        ILogger<FunctionAppService> logger,
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

    public async Task<IEnumerable<SubscriptionResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _domain.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<SubscriptionResponse>>(entities);
    }

    public async Task<SubscriptionResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _domain.GetByIdAsync(id, ct);
        return _mapper.Map<SubscriptionResponse>(entity);
    }

    public async Task<SubscriptionResponse> GetByTenantIdAsync(int tenantId, CancellationToken ct)
    {
        var entity = await _domain.GetByTenantIdAsync(tenantId, ct);
        return _mapper.Map<SubscriptionResponse>(entity);
    }

    public async Task<ListPageResponse<SubscriptionResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _domain.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<SubscriptionResponse>>(paged);
    }

    public async Task<IEnumerable<SubscriptionResponse>> GetActiveSubscriptionsAsync(CancellationToken ct)
    {
        var entities = await _domain.GetActiveSubscriptionsAsync(ct);
        return _mapper.Map<IEnumerable<SubscriptionResponse>>(entities);
    }

    public async Task<IEnumerable<SubscriptionResponse>> GetExpiringSubscriptionsAsync(int daysBeforeExpiration, CancellationToken ct)
    {
        var entities = await _domain.GetExpiringSubscriptionsAsync(daysBeforeExpiration, ct);
        return _mapper.Map<IEnumerable<SubscriptionResponse>>(entities);
    }

    public async Task<bool> CreateAsync(CreateSubscriptionRequest request, CancellationToken ct)
    {
        // Verifica se já existe uma subscription ativa para o tenant
        var exists = await _domain.ExistsByTenantIdAsync(request.TenantId, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Subscription.Create.TenantAlreadyHasSubscription"), 400);
            return false;
        }

        var entity = new SubscriptionEntity(
            request.TenantId,
            request.PlanId,
            request.CurrentPeriodStart,
            request.CurrentPeriodEnd,
            request.TrialStart,
            request.TrialEnd,
            request.StripeCustomerId,
            _currentUser.GetUserId()
        );

        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateSubscriptionRequest request, CancellationToken ct)
    {
        var entity = await _domain.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Subscription.Update.ResourceNotFound"), 404);
            return false;
        }

        entity.Update(
            request.PlanId,
            request.CurrentPeriodStart,
            request.CurrentPeriodEnd,
            request.TrialStart,
            request.TrialEnd,
            _currentUser.GetUserId()
        );

        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var entity = await _domain.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Subscription.Activate.ResourceNotFound"), 404);
            return false;
        }

        entity.Activate(_currentUser.GetUserId());
        return await _domain.ActivateAsync(entity, ct);
    }

    public async Task<bool> DeactivateAsync(int id, CancellationToken ct)
    {
        var entity = await _domain.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Subscription.Deactivate.ResourceNotFound"), 404);
            return false;
        }

        entity.Deactivate(_currentUser.GetUserId());
        return await _domain.DeactivateAsync(entity, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await _domain.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Subscription.Delete.ResourceNotFound"), 404);
            return false;
        }

        entity.Delete(_currentUser.GetUserId());
        return await _domain.DeleteAsync(entity, ct);
    }

    public async Task<bool> CancelAsync(int id, CancelSubscriptionRequest request, CancellationToken ct)
    {
        var entity = await _domain.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Subscription.Cancel.ResourceNotFound"), 404);
            return false;
        }

        entity.Cancel(request.CancellationReason, request.CancelAtPeriodEnd, _currentUser.GetUserId());
        return await _domain.CancelAsync(entity, ct);
    }

    public async Task<bool> RenewAsync(int id, RenewSubscriptionRequest request, CancellationToken ct)
    {
        var entity = await _domain.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Subscription.Renew.ResourceNotFound"), 404);
            return false;
        }

        entity.Renew(request.NewPeriodEnd, _currentUser.GetUserId());
        return await _domain.RenewAsync(entity, ct);
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
            _notify.Add(_localization.GetMessage("Application.Service.Subscription.BulkUpload.EmptyFile"), 400);
            return false;
        }

        // Processa cada item
        return await ProcessBulkItemsAsync(items, ct);
    }

    private List<BulkUploadSubscriptionItem> ReadCsvFile(IFormFile file)
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
            var records = new List<BulkUploadSubscriptionItem>();

            csv.Read();
            csv.ReadHeader();

            int rowCount = 0;
            int maxRows = DomainExtensions.GetMaxCsvRows();

            while (csv.Read() && rowCount < maxRows)
            {
                try
                {
                    var record = csv.GetRecord<BulkUploadSubscriptionItem>();
                    if (record != null)
                    {
                        // Valida se os campos não contêm conteúdo perigoso
                        if (record.TenantId <= 0 )
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.Subscription.ReadCsvFile.TenantId.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }
                        if (record.PlanId <= 0)
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.Subscription.ReadCsvFile.PlanId.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }
                        if (record.CurrentPeriodStart <= DateTime.MinValue)
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.Subscription.ReadCsvFile.CurrentPeriodStart.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }
                        if (record.CurrentPeriodEnd <= DateTime.MinValue)
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.Subscription.ReadCsvFile.CurrentPeriodEnd.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }
                        if (record.CurrentPeriodEnd < record.CurrentPeriodStart)
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.Subscription.ReadCsvFile.CurrentPeriodEnd.CurrentPeriodStart.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        records.Add(record);
                    }
                    rowCount++;
                }
                catch (CsvHelperException ex)
                {
                    // Log linha com erro mas continua processamento
                    _logger.LogWarning(ex, "Erro ao processar linha {RowNumber} do CSV de Subscriptions", rowCount + 2);
                    _notify.Add(_localization.GetMessage("Application.Service.Subscription.ReadCsvFile.CsvHelperException", rowCount + 2), 400);
                    rowCount++;
                    continue;
                }
            }

            if (rowCount >= maxRows)
            {
                _notify.Add(_localization.GetMessage("Application.Service.Subscription.ReadCsvFile.MaxRows", maxRows), 400);
                return null;
            }

            return records;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao ler arquivo CSV de Subscriptions: {Message}", ex.Message);
            _notify.Add(_localization.GetMessage("Application.Service.Subscription.ReadCsvFile.Exception"), 400);
            return null;
        }
    }

    private async Task<bool> ProcessBulkItemsAsync(List<BulkUploadSubscriptionItem> items, CancellationToken ct)
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
            var exists = await _repo.ExistsByTenantIdAsync(tenantId, ct);
            if (exists)
            {
                _notify.Add(_localization.GetMessage("Application.Service.Subscription.ProcessBulkItems.ExistsByTenantId", item.TenantId), 400);
                hasErrors = true;
                continue;
            }

            // Cria a entidade
            var entity = new SubscriptionEntity(tenantId, item.PlanId, item.CurrentPeriodStart, item.CurrentPeriodEnd, item.TrialStart, item.TrialEnd, item.StripeCustomerId, _currentUser.GetUserId());

            // Tenta criar no domínio
            var success = await _domain.CreateAsync(entity, ct);

            if (!success)
            {
                _notify.Add(_localization.GetMessage("Application.Service.Subscription.ProcessBulkItems.FailedToCreate"), 400);
                hasErrors = true;
            }
        }

        return !hasErrors;
    }

    private bool ValidateBulkItem(BulkUploadSubscriptionItem item)
    {
        if (item.TenantId <= 0 )
        {
            _notify.Add(_localization.GetMessage("Application.Service.Subscription.ValidateBulkItem.TenantId", item.TenantId), 400);
            return false;
        }
        if (item.PlanId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Subscription.ValidateBulkItem.PlanId", item.PlanId), 400);
            return false;
        }
        return true;
    }
}
