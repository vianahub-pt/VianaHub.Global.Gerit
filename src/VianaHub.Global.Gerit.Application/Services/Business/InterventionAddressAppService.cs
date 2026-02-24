using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionAddress;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionAddress;
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
/// Serviço de aplicação para InterventionAddress
/// </summary>
public class InterventionAddressAppService : IInterventionAddressAppService
{
    private readonly IInterventionAddressDataRepository _repo;
    private readonly IInterventionAddressDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;
    private readonly IFileValidationService _fileValidation;

    public InterventionAddressAppService(
        IInterventionAddressDataRepository repo,
        IInterventionAddressDomainService domain,
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

    public async Task<IEnumerable<InterventionAddressResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        if (entities == null)
            return Enumerable.Empty<InterventionAddressResponse>();
        
        return _mapper.Map<IEnumerable<InterventionAddressResponse>>(entities);
    }

    public async Task<InterventionAddressResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionAddress.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<InterventionAddressResponse>(entity);
    }

    public async Task<ListPageResponse<InterventionAddressResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<InterventionAddressResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateInterventionAddressRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var exists = await _repo.ExistsByInterventionAndAddressAsync(tenantId, request.InterventionId, request.Street, request.City, request.PostalCode, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionAddress.Create.ResourceAlreadyExists"), 409);
            return false;
        }

        var entity = new InterventionAddressEntity(
            tenantId,
            request.InterventionId,
            request.AddressTypeId,
            request.CountryCode,
            request.Street,
            request.StreetNumber,
            request.Complement,
            request.Neighborhood,
            request.City,
            request.District,
            request.PostalCode,
            request.Latitude,
            request.Longitude,
            request.Notes,
            request.IsPrimary,
            _currentUser.GetUserId());

        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateInterventionAddressRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionAddress.Update.ResourceNotFound"), 410);
            return false;
        }

        entity.UpdateAddressInfo(
            request.AddressTypeId,
            request.CountryCode,
            request.Street,
            request.StreetNumber,
            request.Complement,
            request.Neighborhood,
            request.City,
            request.District,
            request.PostalCode,
            request.Latitude,
            request.Longitude,
            request.Notes,
            _currentUser.GetUserId());

        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionAddress.Activate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.InterventionAddress.Deactivate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.InterventionAddress.Delete.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.InterventionAddress.BulkUpload.EmptyFile"), 400);
            return false;
        }

        return await ProcessBulkItemsAsync(items, ct);
    }

    private List<BulkUploadInterventionAddressItem> ReadCsvFile(IFormFile file)
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
            var records = new List<BulkUploadInterventionAddressItem>();

            csv.Read();
            csv.ReadHeader();

            int rowCount = 0;
            int maxRows = DomainExtensions.GetMaxCsvRows();

            while (csv.Read() && rowCount < maxRows)
            {
                try
                {
                    var record = csv.GetRecord<BulkUploadInterventionAddressItem>();
                    if (record != null)
                    {
                        record.Street = record.Street?.SanitizeCsvInput().NormalizeUtf8();
                        record.Neighborhood = record.Neighborhood?.SanitizeCsvInput().NormalizeUtf8();
                        record.City = record.City?.SanitizeCsvInput().NormalizeUtf8();
                        record.PostalCode = record.PostalCode?.SanitizeCsvInput().NormalizeUtf8();
                        record.District = record.District?.SanitizeCsvInput().NormalizeUtf8();
                        record.CountryCode = record.CountryCode?.SanitizeCsvInput().NormalizeUtf8();

                        if (!string.IsNullOrEmpty(record.Street) && !record.Street.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.InterventionAddress.ReadCsvFile.Street.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        if (!string.IsNullOrEmpty(record.City) && !record.City.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.InterventionAddress.ReadCsvFile.City.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        if (!string.IsNullOrEmpty(record.PostalCode) && !record.PostalCode.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.InterventionAddress.ReadCsvFile.PostalCode.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        records.Add(record);
                    }
                    rowCount++;
                }
                catch (CsvHelperException)
                {
                    _notify.Add(_localization.GetMessage("Application.Service.InterventionAddress.ReadCsvFile.CsvHelperException", rowCount + 2), 400);
                    rowCount++;
                    continue;
                }
            }

            if (rowCount >= maxRows)
            {
                _notify.Add(_localization.GetMessage("Application.Service.InterventionAddress.ReadCsvFile.MaxRows", maxRows), 400);
                return null;
            }

            return records;
        }
        catch (Exception)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionAddress.ReadCsvFile.Exception"), 400);
            return null;
        }
    }

    private async Task<bool> ProcessBulkItemsAsync(List<BulkUploadInterventionAddressItem> items, CancellationToken ct)
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

            var exists = await _repo.ExistsByInterventionAndAddressAsync(tenantId, item.InterventionId, item.Street, item.City, item.PostalCode, ct);
            if (exists)
            {
                _notify.Add(_localization.GetMessage("Application.Service.InterventionAddress.ProcessBulkItems.ExistsByAddress", item.Street), 400);
                hasErrors = true;
                continue;
            }

            var entity = new InterventionAddressEntity(
                tenantId,
                item.InterventionId,
                item.AddressTypeId,
                item.CountryCode,
                item.Street,
                item.StreetNumber,
                item.Complement,
                item.Neighborhood,
                item.City,
                item.District,
                item.PostalCode,
                item.Latitude,
                item.Longitude,
                item.Notes,
                item.IsPrimary,
                _currentUser.GetUserId());

            var success = await _domain.CreateAsync(entity, ct);

            if (!success)
            {
                _notify.Add(_localization.GetMessage("Application.Service.InterventionAddress.ProcessBulkItems.FailedToCreate", item.Street), 400);
                hasErrors = true;
            }
        }

        return !hasErrors;
    }

    private bool ValidateBulkItem(BulkUploadInterventionAddressItem item)
    {
        if (string.IsNullOrWhiteSpace(item.Street))
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionAddress.ValidateBulkItem.Street", item.InterventionId), 400);
            return false;
        }

        if (string.IsNullOrWhiteSpace(item.City))
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionAddress.ValidateBulkItem.City", item.InterventionId), 400);
            return false;
        }

        if (string.IsNullOrWhiteSpace(item.PostalCode))
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionAddress.ValidateBulkItem.PostalCode", item.InterventionId), 400);
            return false;
        }

        if (string.IsNullOrWhiteSpace(item.CountryCode))
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionAddress.ValidateBulkItem.CountryCode", item.InterventionId), 400);
            return false;
        }

        if (item.InterventionId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.InterventionAddress.ValidateBulkItem.InterventionId", item.InterventionId), 400);
            return false;
        }

        return true;
    }
}
