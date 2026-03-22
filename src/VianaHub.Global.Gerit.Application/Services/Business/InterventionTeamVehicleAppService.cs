using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionTeamVehicles;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionTeamVehicles;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Application.Interfaces.Common;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Helpers;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Application.Services.Business;

public class InterventionTeamVehicleAppService : IInterventionTeamVehiclesAppService
{
    private readonly IInterventionTeamVehicleDataRepository _repo;
    private readonly IInterventionTeamVehicleDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;
    private readonly IFileValidationService _fileValidation;

    public InterventionTeamVehicleAppService(
        IInterventionTeamVehicleDataRepository repo,
        IInterventionTeamVehicleDomainService domain,
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

    public async Task<IEnumerable<InterventionTeamVehicleResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<InterventionTeamVehicleResponse>>(entities);
    }

    public async Task<InterventionTeamVehicleResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamVehicle.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<InterventionTeamVehicleResponse>(entity);
    }

    public async Task<ListPageResponse<InterventionTeamVehicleResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.IsActive, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<InterventionTeamVehicleResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateInterventionTeamVehicleRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var exists = await _repo.ExistsByIdAsync(tenantId, request.InterventionTeamId, request.VehicleId, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamVehicle.Create.ResourceAlreadyExists", request.InterventionTeamId, request.VehicleId), 409);
            return false;
        }

        var entity = new InterventionTeamVehicleEntity(tenantId, request.InterventionTeamId, request.VehicleId, _currentUser.GetUserId());
        entity.Update(request.InterventionTeamId, request.VehicleId, _currentUser.GetUserId());
        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateInterventionTeamVehicleRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamVehicle.Update.ResourceNotFound"), 410);
            return false;
        }

        entity.Update(request.InterventionTeamId, request.VehicleId, _currentUser.GetUserId());
        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamVehicle.Activate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamVehicle.Deactivate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamVehicle.Delete.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamVehicle.BulkUpload.EmptyFile"), 400);
            return false;
        }

        return await ProcessBulkItemsAsync(items, ct);
    }

    private List<BulkUploadInterventionTeamVehicleItem> ReadCsvFile(IFormFile file)
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
            var records = new List<BulkUploadInterventionTeamVehicleItem>();

            csv.Read();
            csv.ReadHeader();

            if (csv.HeaderRecord != null && csv.HeaderRecord.Length == 1 && csv.HeaderRecord[0].Contains(','))
            {
                _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamVehicle.ReadCsvFile.InvalidDelimiter"), 400);
                return null;
            }

            int rowCount = 0;
            int maxRows = DomainExtensions.GetMaxCsvRows();

            while (csv.Read() && rowCount < maxRows)
            {
                try
                {
                    var record = csv.GetRecord<BulkUploadInterventionTeamVehicleItem>();
                    if (record != null)
                    {
                        records.Add(record);
                    }
                    rowCount++;
                }
                catch (CsvHelperException)
                {
                    _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamVehicle.ReadCsvFile.CsvHelperException", rowCount + 2), 400);
                    rowCount++;
                    continue;
                }
            }

            if (rowCount >= maxRows)
            {
                _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamVehicle.ReadCsvFile.MaxRows", maxRows), 400);
                return null;
            }

            return records;
        }
        catch (Exception)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamVehicle.ReadCsvFile.Exception"), 400);
            return null;
        }
    }

    private async Task<bool> ProcessBulkItemsAsync(List<BulkUploadInterventionTeamVehicleItem> items, CancellationToken ct)
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

            var exists = await _repo.ExistsByIdAsync(tenantId, item.InterventionTeamId, item.VehicleId, ct);
            if (exists)
            {
                _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamVehicle.ProcessBulkItems.ExistsByInterventionAndVehicle", item.InterventionTeamId, item.VehicleId), 400);
                hasErrors = true;
                continue;
            }

            var entity = new InterventionTeamVehicleEntity(tenantId, item.InterventionTeamId, item.VehicleId, _currentUser.GetUserId());
            entity.Update(item.InterventionTeamId, item.VehicleId, _currentUser.GetUserId());

            var success = await _domain.CreateAsync(entity, ct);

            if (!success)
            {
                _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamVehicle.ProcessBulkItems.FailedToCreate", item.InterventionTeamId, item.VehicleId), 400);
                hasErrors = true;
            }
        }

        return !hasErrors;
    }

    private bool ValidateBulkItem(BulkUploadInterventionTeamVehicleItem item)
    {
        if (item.InterventionTeamId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamVehicle.ValidateBulkItem.InterventionTeamId", item.InterventionTeamId), 400);
            return false;
        }

        if (item.VehicleId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeamVehicle.ValidateBulkItem.VehicleId", item.VehicleId), 400);
            return false;
        }

        return true;
    }
}
