using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.TeamMember;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.TeamMember;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Application.Interfaces.Common;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Helpers;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Application.Services.Business;

public class TeamMemberAppService : ITeamMemberAppService
{
    private readonly ITeamMemberDataRepository _repo;
    private readonly ITeamMemberDomainService _domain;
    private readonly IFunctionDataRepository _functionRepo;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;
    private readonly IFileValidationService _fileValidation;

    public TeamMemberAppService(
        ITeamMemberDataRepository repo,
        ITeamMemberDomainService domain,
        IFunctionDataRepository functionRepo,
        IMapper mapper,
        INotify notify,
        ILocalizationService localization,
        ICurrentUserService currentUser,
        IFileValidationService fileValidation)
    {
        _repo = repo;
        _domain = domain;
        _functionRepo = functionRepo;
        _mapper = mapper;
        _notify = notify;
        _localization = localization;
        _currentUser = currentUser;
        _fileValidation = fileValidation;
    }

    public async Task<IEnumerable<TeamMemberResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<TeamMemberResponse>>(entities);
    }

    public async Task<TeamMemberResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMember.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<TeamMemberResponse>(entity);
    }

    public async Task<ListPageResponse<TeamMemberResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.IsActive, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<TeamMemberResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateTeamMemberRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var exists = await _repo.ExistsByTaxNumberAsync(tenantId, request.TaxNumber, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMember.Create.ResourceAlreadyExists"), 409);
            return false;
        }

        var entity = new TeamMemberEntity(tenantId, request.FunctionId, request.Name, request.TaxNumber, _currentUser.GetUserId());
        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateTeamMemberRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMember.Update.ResourceNotFound"), 410);
            return false;
        }

        entity.Update(request.FunctionId, request.Name, _currentUser.GetUserId());
        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMember.Activate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.TeamMember.Deactivate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.TeamMember.Delete.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.TeamMember.BulkUpload.EmptyFile"), 400);
            return false;
        }

        return await ProcessBulkItemsAsync(items, ct);
    }

    private List<BulkUploadTeamMemberItem> ReadCsvFile(IFormFile file)
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
            var records = new List<BulkUploadTeamMemberItem>();

            csv.Read();
            csv.ReadHeader();

            int rowCount = 0;
            int maxRows = DomainExtensions.GetMaxCsvRows();

            while (csv.Read() && rowCount < maxRows)
            {
                try
                {
                    var record = csv.GetRecord<BulkUploadTeamMemberItem>();
                    if (record != null)
                    {
                        record.Name = record.Name?.SanitizeCsvInput().NormalizeUtf8();
                        record.TaxNumber = record.TaxNumber?.SanitizeCsvInput().NormalizeUtf8();

                        if (!string.IsNullOrEmpty(record.Name) && !record.Name.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.TeamMember.ReadCsvFile.Name.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        if (!string.IsNullOrEmpty(record.TaxNumber) && !record.TaxNumber.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.TeamMember.ReadCsvFile.TaxNumber.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        records.Add(record);
                    }
                    rowCount++;
                }
                catch (CsvHelperException)
                {
                    _notify.Add(_localization.GetMessage("Application.Service.TeamMember.ReadCsvFile.CsvHelperException", rowCount + 2), 400);
                    rowCount++;
                    continue;
                }
            }

            if (rowCount >= maxRows)
            {
                _notify.Add(_localization.GetMessage("Application.Service.TeamMember.ReadCsvFile.MaxRows", maxRows), 400);
                return null;
            }

            return records;
        }
        catch (Exception)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMember.ReadCsvFile.Exception"), 400);
            return null;
        }
    }

    private async Task<bool> ProcessBulkItemsAsync(List<BulkUploadTeamMemberItem> items, CancellationToken ct)
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

            // Valida se a função existe
            var functionExists = await _functionRepo.ExistsByIdAsync(item.FunctionId, ct);
            if (!functionExists)
            {
                _notify.Add(_localization.GetMessage("Application.Service.TeamMember.ProcessBulkItems.FunctionNotFound", item.FunctionId, item.Name), 400);
                hasErrors = true;
                continue;
            }

            var exists = await _repo.ExistsByTaxNumberAsync(tenantId, item.TaxNumber, ct);
            if (exists)
            {
                _notify.Add(_localization.GetMessage("Application.Service.TeamMember.ProcessBulkItems.ExistsByTaxNumber", item.TaxNumber), 400);
                hasErrors = true;
                continue;
            }

            var entity = new TeamMemberEntity(tenantId, item.FunctionId, item.Name, item.TaxNumber, _currentUser.GetUserId());

            var success = await _domain.CreateAsync(entity, ct);

            if (!success)
            {
                _notify.Add(_localization.GetMessage("Application.Service.TeamMember.ProcessBulkItems.FailedToCreate", item.Name), 400);
                hasErrors = true;
            }
        }

        return !hasErrors;
    }

    private bool ValidateBulkItem(BulkUploadTeamMemberItem item)
    {
        if (string.IsNullOrWhiteSpace(item.Name))
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMember.ValidateBulkItem.Name", item.Name), 400);
            return false;
        }

        if (string.IsNullOrWhiteSpace(item.TaxNumber))
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMember.ValidateBulkItem.TaxNumber", item.Name), 400);
            return false;
        }

        if (item.FunctionId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMember.ValidateBulkItem.FunctionId", item.Name), 400);
            return false;
        }

        return true;
    }
}
