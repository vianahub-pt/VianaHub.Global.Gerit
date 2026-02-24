using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionTeams;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionTeams;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Application.Interfaces.Common;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Helpers;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Application.Services.Business;

public class InterventionTeamAppService : IInterventionTeamsAppService
{
    private readonly IInterventionTeamDataRepository _repo;
    private readonly IInterventionTeamDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;
    private readonly IFileValidationService _fileValidation;

    public InterventionTeamAppService(
        IInterventionTeamDataRepository repo,
        IInterventionTeamDomainService domain,
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

    public async Task<IEnumerable<InterventionTeamResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<InterventionTeamResponse>>(entities);
    }

    public async Task<InterventionTeamResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeam.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<InterventionTeamResponse>(entity);
    }

    public async Task<ListPageResponse<InterventionTeamResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<InterventionTeamResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateInterventionTeamRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var exists = await _repo.ExistsByIdAsync(tenantId, request.InterventionId, request.TeamId, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeam.Create.ResourceAlreadyExists"), 409);
            return false;
        }

        var entity = new InterventionTeamEntity(tenantId, request.InterventionId, request.TeamId, _currentUser.GetUserId());
        entity.Update(request.InterventionId, request.TeamId, _currentUser.GetUserId());
        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateInterventionTeamRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeam.Update.ResourceNotFound"), 410);
            return false;
        }

        entity.Update(request.InterventionId, request.TeamId, _currentUser.GetUserId());
        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeam.Activate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeam.Deactivate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeam.Delete.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeam.BulkUpload.EmptyFile"), 400);
            return false;
        }

        return await ProcessBulkItemsAsync(items, ct);
    }

    private List<BulkUploadInterventionTeamItem> ReadCsvFile(IFormFile file)
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
            var records = new List<BulkUploadInterventionTeamItem>();

            csv.Read();
            csv.ReadHeader();

            // If header was read as a single column that contains commas, the delimiter is likely incorrect
            if (csv.HeaderRecord != null && csv.HeaderRecord.Length == 1 && csv.HeaderRecord[0].Contains(','))
            {
                _notify.Add(_localization.GetMessage("Application.Service.InterventionTeam.ReadCsvFile.InvalidDelimiter"), 400);
                return null;
            }

            int rowCount = 0;
            int maxRows = DomainExtensions.GetMaxCsvRows();

            while (csv.Read() && rowCount < maxRows)
            {
                try
                {
                    var record = csv.GetRecord<BulkUploadInterventionTeamItem>();
                    if (record != null)
                    {
                        records.Add(record);
                    }
                    rowCount++;
                }
                catch (CsvHelperException)
                {
                    _notify.Add(_localization.GetMessage("Application.Service.InterventionTeam.ReadCsvFile.CsvHelperException", rowCount + 2), 400);
                    rowCount++;
                    continue;
                }
            }

            if (rowCount >= maxRows)
            {
                _notify.Add(_localization.GetMessage("Application.Service.InterventionTeam.ReadCsvFile.MaxRows", maxRows), 400);
                return null;
            }

            return records;
        }
        catch (Exception)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeam.ReadCsvFile.Exception"), 400);
            return null;
        }
    }

    private async Task<bool> ProcessBulkItemsAsync(List<BulkUploadInterventionTeamItem> items, CancellationToken ct)
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

            var exists = await _repo.ExistsByIdAsync(tenantId, item.InterventionId, item.TeamId, ct);
            if (exists)
            {
                _notify.Add(_localization.GetMessage("Application.Service.InterventionTeam.ProcessBulkItems.ExistsByInterventionAndTeam", item.InterventionId, item.TeamId), 400);
                hasErrors = true;
                continue;
            }

            var entity = new InterventionTeamEntity(tenantId, item.InterventionId, item.TeamId, _currentUser.GetUserId());
            entity.Update(item.InterventionId, item.TeamId, _currentUser.GetUserId());

            var success = await _domain.CreateAsync(entity, ct);

            if (!success)
            {
                _notify.Add(_localization.GetMessage("Application.Service.InterventionTeam.ProcessBulkItems.FailedToCreate", item.InterventionId, item.TeamId), 400);
                hasErrors = true;
            }
        }

        return !hasErrors;
    }

    private bool ValidateBulkItem(BulkUploadInterventionTeamItem item)
    {
        if (item.InterventionId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeam.ValidateBulkItem.InterventionId", item.InterventionId), 400);
            return false;
        }

        if (item.TeamId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionTeam.ValidateBulkItem.TeamId", item.TeamId), 400);
            return false;
        }

        return true;
    }
}
