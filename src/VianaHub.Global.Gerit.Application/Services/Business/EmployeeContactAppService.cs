using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.EmployeeContact;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.EmployeeContact;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Application.Interfaces.Common;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Helpers;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Application.Services.Business;

public class EmployeeContactAppService : IEmployeeContactAppService
{
    private readonly IEmployeeContactDataRepository _repo;
    private readonly IEmployeeContactDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;
    private readonly IFileValidationService _fileValidation;

    public EmployeeContactAppService(
        IEmployeeContactDataRepository repo,
        IEmployeeContactDomainService domain,
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

    public async Task<IEnumerable<EmployeeContactResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<EmployeeContactResponse>>(entities);
    }

    public async Task<EmployeeContactResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.EmployeeContact.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<EmployeeContactResponse>(entity);
    }

    public async Task<ListPageResponse<EmployeeContactResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.IsActive, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<EmployeeContactResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateEmployeeContactRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var exists = await _repo.ExistsByEmailAsync(tenantId, request.EmployeeId, request.Email, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.EmployeeContact.Create.ResourceAlreadyExists"), 409);
            return false;
        }

        var entity = new EmployeeContactEntity(
            tenantId, 
            request.EmployeeId, 
            request.Name, 
            request.Email, 
            request.Phone, 
            request.IsPrimary, 
            _currentUser.GetUserId());

        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateEmployeeContactRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.EmployeeContact.Update.ResourceNotFound"), 410);
            return false;
        }

        var tenantId = _currentUser.GetTenantId();
        var emailExists = await _repo.ExistsByEmailForUpdateAsync(tenantId, entity.EmployeeId, request.Email, id, ct);
        if (emailExists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.EmployeeContact.Update.EmailAlreadyExists"), 409);
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
            _notify.Add(_localization.GetMessage("Application.Service.EmployeeContact.Activate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.EmployeeContact.Deactivate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.EmployeeContact.Delete.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.EmployeeContact.BulkUpload.EmptyFile"), 400);
            return false;
        }

        return await ProcessBulkItemsAsync(items, ct);
    }

    private List<BulkUploadEmployeeContactItem> ReadCsvFile(IFormFile file)
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
            var records = new List<BulkUploadEmployeeContactItem>();

            csv.Read();
            csv.ReadHeader();

            int rowCount = 0;
            int maxRows = DomainExtensions.GetMaxCsvRows();

            while (csv.Read() && rowCount < maxRows)
            {
                try
                {
                    var record = csv.GetRecord<BulkUploadEmployeeContactItem>();
                    if (record != null)
                    {
                        record.Name = record.Name?.SanitizeCsvInput().NormalizeUtf8();
                        record.Email = record.Email?.SanitizeCsvInput().NormalizeUtf8();
                        record.Phone = record.Phone?.SanitizeCsvInput().NormalizeUtf8();

                        if (!string.IsNullOrEmpty(record.Name) && !record.Name.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.EmployeeContact.ReadCsvFile.Name.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        if (!string.IsNullOrEmpty(record.Email) && !record.Email.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.EmployeeContact.ReadCsvFile.Email.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        if (!string.IsNullOrEmpty(record.Phone) && !record.Phone.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.EmployeeContact.ReadCsvFile.Phone.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        records.Add(record);
                    }
                    rowCount++;
                }
                catch (CsvHelperException)
                {
                    _notify.Add(_localization.GetMessage("Application.Service.EmployeeContact.ReadCsvFile.CsvHelperException", rowCount + 2), 400);
                    rowCount++;
                    continue;
                }
            }

            if (rowCount >= maxRows)
            {
                _notify.Add(_localization.GetMessage("Application.Service.EmployeeContact.ReadCsvFile.MaxRows", maxRows), 400);
                return null;
            }

            return records;
        }
        catch (Exception)
        {
            _notify.Add(_localization.GetMessage("Application.Service.EmployeeContact.ReadCsvFile.Exception"), 400);
            return null;
        }
    }

    private async Task<bool> ProcessBulkItemsAsync(List<BulkUploadEmployeeContactItem> items, CancellationToken ct)
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

            var exists = await _repo.ExistsByEmailAsync(tenantId, item.EmployeeId, item.Email, ct);
            if (exists)
            {
                _notify.Add(_localization.GetMessage("Application.Service.EmployeeContact.ProcessBulkItems.ExistsByEmail", item.Email), 400);
                hasErrors = true;
                continue;
            }

            var entity = new EmployeeContactEntity(
                tenantId, 
                item.EmployeeId, 
                item.Name, 
                item.Email, 
                item.Phone, 
                item.IsPrimary, 
                _currentUser.GetUserId());

            var success = await _domain.CreateAsync(entity, ct);

            if (!success)
            {
                _notify.Add(_localization.GetMessage("Application.Service.EmployeeContact.ProcessBulkItems.FailedToCreate", item.Name), 400);
                hasErrors = true;
            }
        }

        return !hasErrors;
    }

    private bool ValidateBulkItem(BulkUploadEmployeeContactItem item)
    {
        if (string.IsNullOrWhiteSpace(item.Name))
        {
            _notify.Add(_localization.GetMessage("Application.Service.EmployeeContact.ValidateBulkItem.Name", item.Name), 400);
            return false;
        }

        if (string.IsNullOrWhiteSpace(item.Email))
        {
            _notify.Add(_localization.GetMessage("Application.Service.EmployeeContact.ValidateBulkItem.Email", item.Name), 400);
            return false;
        }

        if (item.EmployeeId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.EmployeeContact.ValidateBulkItem.EmployeeId", item.Name), 400);
            return false;
        }

        return true;
    }
}
