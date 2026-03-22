using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.TeamMemberContact;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.TeamMemberContact;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Application.Interfaces.Common;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Helpers;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Application.Services.Business;

public class TeamMemberContactAppService : ITeamMemberContactAppService
{
    private readonly ITeamMemberContactDataRepository _repo;
    private readonly ITeamMemberContactDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;
    private readonly IFileValidationService _fileValidation;

    public TeamMemberContactAppService(
        ITeamMemberContactDataRepository repo,
        ITeamMemberContactDomainService domain,
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

    public async Task<IEnumerable<TeamMemberContactResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<TeamMemberContactResponse>>(entities);
    }

    public async Task<TeamMemberContactResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberContact.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<TeamMemberContactResponse>(entity);
    }

    public async Task<ListPageResponse<TeamMemberContactResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.IsActive, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<TeamMemberContactResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateTeamMemberContactRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var exists = await _repo.ExistsByEmailAsync(tenantId, request.TeamMemberId, request.Email, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberContact.Create.ResourceAlreadyExists"), 409);
            return false;
        }

        var entity = new TeamMemberContactEntity(
            tenantId, 
            request.TeamMemberId, 
            request.Name, 
            request.Email, 
            request.Phone, 
            request.IsPrimary, 
            _currentUser.GetUserId());

        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateTeamMemberContactRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberContact.Update.ResourceNotFound"), 410);
            return false;
        }

        var tenantId = _currentUser.GetTenantId();
        var emailExists = await _repo.ExistsByEmailForUpdateAsync(tenantId, entity.TeamMemberId, request.Email, id, ct);
        if (emailExists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberContact.Update.EmailAlreadyExists"), 409);
            return false;
        }

        entity.UpdateContactInfo(request.Name, request.Email, request.Phone, _currentUser.GetUserId());

        if (request.IsPrimary && !entity.IsPrimary)
        {
            entity.SetAsPrimary(_currentUser.GetUserId());
        }
        else if (!request.IsPrimary && entity.IsPrimary)
        {
            entity.RemoveAsPrimary(_currentUser.GetUserId());
        }

        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberContact.Activate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberContact.Deactivate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberContact.Delete.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberContact.BulkUpload.EmptyFile"), 400);
            return false;
        }

        return await ProcessBulkItemsAsync(items, ct);
    }

    private List<BulkUploadTeamMemberContactItem> ReadCsvFile(IFormFile file)
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
            var records = new List<BulkUploadTeamMemberContactItem>();

            csv.Read();
            csv.ReadHeader();

            int rowCount = 0;
            int maxRows = DomainExtensions.GetMaxCsvRows();

            while (csv.Read() && rowCount < maxRows)
            {
                try
                {
                    var record = csv.GetRecord<BulkUploadTeamMemberContactItem>();
                    if (record != null)
                    {
                        record.Name = record.Name?.SanitizeCsvInput().NormalizeUtf8();
                        record.Email = record.Email?.SanitizeCsvInput().NormalizeUtf8();
                        record.Phone = record.Phone?.SanitizeCsvInput().NormalizeUtf8();

                        if (!string.IsNullOrEmpty(record.Name) && !record.Name.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberContact.ReadCsvFile.Name.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        if (!string.IsNullOrEmpty(record.Email) && !record.Email.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberContact.ReadCsvFile.Email.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        if (!string.IsNullOrEmpty(record.Phone) && !record.Phone.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberContact.ReadCsvFile.Phone.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        records.Add(record);
                    }
                    rowCount++;
                }
                catch (CsvHelperException)
                {
                    _notify.Add(_localization.GetMessage("Application.Service.TeamMemberContact.ReadCsvFile.CsvHelperException", rowCount + 2), 400);
                    rowCount++;
                    continue;
                }
            }

            if (rowCount >= maxRows)
            {
                _notify.Add(_localization.GetMessage("Application.Service.TeamMemberContact.ReadCsvFile.MaxRows", maxRows), 400);
                return null;
            }

            return records;
        }
        catch (Exception)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberContact.ReadCsvFile.Exception"), 400);
            return null;
        }
    }

    private async Task<bool> ProcessBulkItemsAsync(List<BulkUploadTeamMemberContactItem> items, CancellationToken ct)
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

            var exists = await _repo.ExistsByEmailAsync(tenantId, item.TeamMemberId, item.Email, ct);
            if (exists)
            {
                _notify.Add(_localization.GetMessage("Application.Service.TeamMemberContact.ProcessBulkItems.ExistsByEmail", item.Email), 400);
                hasErrors = true;
                continue;
            }

            var entity = new TeamMemberContactEntity(
                tenantId, 
                item.TeamMemberId, 
                item.Name, 
                item.Email, 
                item.Phone, 
                item.IsPrimary, 
                _currentUser.GetUserId());

            var success = await _domain.CreateAsync(entity, ct);

            if (!success)
            {
                _notify.Add(_localization.GetMessage("Application.Service.TeamMemberContact.ProcessBulkItems.FailedToCreate", item.Name), 400);
                hasErrors = true;
            }
        }

        return !hasErrors;
    }

    private bool ValidateBulkItem(BulkUploadTeamMemberContactItem item)
    {
        if (string.IsNullOrWhiteSpace(item.Name))
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberContact.ValidateBulkItem.Name", item.Name), 400);
            return false;
        }

        if (string.IsNullOrWhiteSpace(item.Email))
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberContact.ValidateBulkItem.Email", item.Name), 400);
            return false;
        }

        if (item.TeamMemberId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberContact.ValidateBulkItem.TeamMemberId", item.Name), 400);
            return false;
        }

        return true;
    }
}
