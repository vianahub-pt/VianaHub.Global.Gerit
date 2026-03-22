using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionTeamEquipments;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionTeamEquipments;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Application.Interfaces.Common;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Helpers;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Application.Services.Business;

public class InterventionTeamEquipmentAppService : IInterventionTeamEquipmentsAppService
{
    private readonly IInterventionTeamEquipmentDataRepository _repo;
    private readonly IInterventionTeamEquipmentDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;
    private readonly IFileValidationService _fileValidation;

    public InterventionTeamEquipmentAppService(
        IInterventionTeamEquipmentDataRepository repo,
        IInterventionTeamEquipmentDomainService domain,
        IMapper mapper,
        INotify notify,
        ILocalizationService localization,
        ICurrentUserService currentUser,
        IFileValidationService fileValidation)
    {
        _repo = repo;
        _domain = domain;
        _mapper = mapper;
        _notify = notify;
        _localization = localization;
        _currentUser = currentUser;
        _fileValidation = fileValidation;
    }

    public async Task<IEnumerable<InterventionTeamEquipmentResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<InterventionTeamEquipmentResponse>>(entities);
    }

    public async Task<InterventionTeamEquipmentResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamEquipment.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<InterventionTeamEquipmentResponse>(entity);
    }

    public async Task<ListPageResponse<InterventionTeamEquipmentResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.IsActive, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<InterventionTeamEquipmentResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateInterventionTeamEquipmentRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var exists = await _repo.ExistsByIdAsync(tenantId, request.InterventionTeamId, request.EquipmentId, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamEquipment.Create.ResourceAlreadyExists", request.InterventionTeamId, request.EquipmentId), 409);
            return false;
        }

        var entity = new InterventionTeamEquipmentEntity(tenantId, request.InterventionTeamId, request.EquipmentId, _currentUser.GetUserId());
        entity.Update(request.InterventionTeamId, request.EquipmentId, _currentUser.GetUserId());
        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateInterventionTeamEquipmentRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamEquipment.Update.ResourceNotFound"), 410);
            return false;
        }

        entity.Update(request.InterventionTeamId, request.EquipmentId, _currentUser.GetUserId());
        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamEquipment.Activate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamEquipment.Deactivate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamEquipment.Delete.ResourceNotFound"), 410);
            return false;
        }

        entity.Delete(_currentUser.GetUserId());
        return await _domain.DeleteAsync(entity, ct);
    }

    public async Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct)
    {
        if (!_fileValidation.ValidateFile(file))
            return false;

        var items = ReadCsvFile(file);
        if (items == null)
            return false;

        if (!items.Any())
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamEquipment.BulkUpload.EmptyFile"), 400);
            return false;
        }

        return await ProcessBulkItemsAsync(items, ct);
    }

    private List<BulkUploadInterventionTeamEquipmentItem> ReadCsvFile(IFormFile file)
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
            var records = new List<BulkUploadInterventionTeamEquipmentItem>();

            csv.Read();
            csv.ReadHeader();

            if (csv.HeaderRecord != null && csv.HeaderRecord.Length == 1 && csv.HeaderRecord[0].Contains(','))
            {
                _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamEquipment.ReadCsvFile.InvalidDelimiter"), 400);
                return null;
            }

            int rowCount = 0;
            int maxRows = DomainExtensions.GetMaxCsvRows();

            while (csv.Read() && rowCount < maxRows)
            {
                try
                {
                    var record = csv.GetRecord<BulkUploadInterventionTeamEquipmentItem>();
                    if (record != null)
                    {
                        records.Add(record);
                    }
                    rowCount++;
                }
                catch (CsvHelperException)
                {
                    _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamEquipment.ReadCsvFile.CsvHelperException", rowCount + 2), 400);
                    rowCount++;
                    continue;
                }
            }

            if (rowCount >= maxRows)
            {
                _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamEquipment.ReadCsvFile.MaxRows", maxRows), 400);
                return null;
            }

            return records;
        }
        catch (Exception)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamEquipment.ReadCsvFile.Exception"), 400);
            return null;
        }
    }

    private async Task<bool> ProcessBulkItemsAsync(List<BulkUploadInterventionTeamEquipmentItem> items, CancellationToken ct)
    {
        var hasErrors = false;
        var tenantId = _currentUser.GetTenantId();

        foreach (var item in items)
        {
            if (!ValidateBulkItem(item))
            {
                hasErrors = true;
                continue;
            }

            var exists = await _repo.ExistsByIdAsync(tenantId, item.InterventionTeamId, item.EquipmentId, ct);
            if (exists)
            {
                _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamEquipment.ProcessBulkItems.ExistsByInterventionAndEquipment", item.InterventionTeamId, item.EquipmentId), 400);
                hasErrors = true;
                continue;
            }

            var entity = new InterventionTeamEquipmentEntity(tenantId, item.InterventionTeamId, item.EquipmentId, _currentUser.GetUserId());
            entity.Update(item.InterventionTeamId, item.EquipmentId, _currentUser.GetUserId());

            var success = await _domain.CreateAsync(entity, ct);

            if (!success)
            {
                _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamEquipment.ProcessBulkItems.FailedToCreate", item.InterventionTeamId, item.EquipmentId), 400);
                hasErrors = true;
            }
        }

        return !hasErrors;
    }

    private bool ValidateBulkItem(BulkUploadInterventionTeamEquipmentItem item)
    {
        if (item.InterventionTeamId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamEquipment.ValidateBulkItem.InterventionTeamId", item.InterventionTeamId), 400);
            return false;
        }

        if (item.EquipmentId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamEquipment.ValidateBulkItem.EquipmentId", item.EquipmentId), 400);
            return false;
        }

        return true;
    }
}
