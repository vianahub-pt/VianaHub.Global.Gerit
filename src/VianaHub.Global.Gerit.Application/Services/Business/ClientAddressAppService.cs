using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientAddress;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientAddress;
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
/// Serviço de aplicação para ClientAddress
/// </summary>
public class ClientAddressAppService : IClientAddressAppService
{
    private readonly IClientAddressDataRepository _repo;
    private readonly IClientAddressDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;
    private readonly IFileValidationService _fileValidation;

    public ClientAddressAppService(
        IClientAddressDataRepository repo,
        IClientAddressDomainService domain,
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

    public async Task<IEnumerable<ClientAddressResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<ClientAddressResponse>>(entities);
    }

    public async Task<ClientAddressResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientAddress.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<ClientAddressResponse>(entity);
    }

    public async Task<ListPageResponse<ClientAddressResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<ClientAddressResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateClientAddressRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        
        var exists = await _repo.ExistsByClientAndAddressTypeAsync(request.ClientId, request.AddressTypeId, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientAddress.Create.ResourceAlreadyExists"), 409);
            return false;
        }

        var entity = new ClientAddressEntity(
            tenantId,
            request.ClientId,
            request.AddressTypeId,
            request.CountryCode,
            request.Street,
            request.Neighborhood,
            request.City,
            request.District,
            request.PostalCode,
            request.StreetNumber,
            request.Complement,
            request.Latitude,
            request.Longitude,
            request.Notes,
            request.IsPrimary,
            _currentUser.GetUserId());

        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateClientAddressRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientAddress.Update.ResourceNotFound"), 410);
            return false;
        }

        var exists = await _repo.ExistsByClientAndAddressTypeExcludingIdAsync(entity.ClientId, request.AddressTypeId, id, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientAddress.Update.ResourceAlreadyExists"), 409);
            return false;
        }

        entity.UpdateAddress(
            request.AddressTypeId,
            request.CountryCode,
            request.Street,
            request.Neighborhood,
            request.City,
            request.District,
            request.PostalCode,
            request.StreetNumber,
            request.Complement,
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
            _notify.Add(_localization.GetMessage("Application.Service.ClientAddress.Activate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.ClientAddress.Deactivate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.ClientAddress.Delete.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.ClientAddress.BulkUpload.EmptyFile"), 400);
            return false;
        }

        return await ProcessBulkItemsAsync(items, ct);
    }

    private List<BulkUploadClientAddressItem> ReadCsvFile(IFormFile file)
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
            var records = new List<BulkUploadClientAddressItem>();

            csv.Read();
            csv.ReadHeader();

            int rowCount = 0;
            int maxRows = DomainExtensions.GetMaxCsvRows();

            while (csv.Read() && rowCount < maxRows)
            {
                try
                {
                    var record = csv.GetRecord<BulkUploadClientAddressItem>();
                    if (record != null)
                    {
                        record.Street = record.Street?.SanitizeCsvInput().NormalizeUtf8();
                        record.City = record.City?.SanitizeCsvInput().NormalizeUtf8();
                        record.PostalCode = record.PostalCode?.SanitizeCsvInput().NormalizeUtf8();
                        record.District = record.District?.SanitizeCsvInput().NormalizeUtf8();
                        record.Neighborhood = record.Neighborhood?.SanitizeCsvInput().NormalizeUtf8();
                        record.CountryCode = record.CountryCode?.SanitizeCsvInput().NormalizeUtf8();
                        record.StreetNumber = record.StreetNumber?.SanitizeCsvInput().NormalizeUtf8();
                        record.Complement = record.Complement?.SanitizeCsvInput().NormalizeUtf8();
                        record.Notes = record.Notes?.SanitizeCsvInput().NormalizeUtf8();

                        if (!string.IsNullOrEmpty(record.Street) && !record.Street.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.ClientAddress.ReadCsvFile.Street.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        if (!string.IsNullOrEmpty(record.City) && !record.City.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.ClientAddress.ReadCsvFile.City.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        if (!string.IsNullOrEmpty(record.PostalCode) && !record.PostalCode.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.ClientAddress.ReadCsvFile.PostalCode.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        records.Add(record);
                    }
                    rowCount++;
                }
                catch (CsvHelperException)
                {
                    _notify.Add(_localization.GetMessage("Application.Service.ClientAddress.ReadCsvFile.CsvHelperException", rowCount + 2), 400);
                    rowCount++;
                    continue;
                }
            }

            if (rowCount >= maxRows)
            {
                _notify.Add(_localization.GetMessage("Application.Service.ClientAddress.ReadCsvFile.MaxRows", maxRows), 400);
                return null;
            }

            return records;
        }
        catch (Exception)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientAddress.ReadCsvFile.Exception"), 400);
            return null;
        }
    }

    private async Task<bool> ProcessBulkItemsAsync(List<BulkUploadClientAddressItem> items, CancellationToken ct)
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

            var exists = await _repo.ExistsByClientAndAddressTypeAsync(item.ClientId, item.AddressTypeId, ct);
            if (exists)
            {
                _notify.Add(_localization.GetMessage("Application.Service.ClientAddress.ProcessBulkItems.ExistsByClientAndType", item.ClientId, item.AddressTypeId), 400);
                hasErrors = true;
                continue;
            }

            var entity = new ClientAddressEntity(
                tenantId,
                item.ClientId,
                item.AddressTypeId,
                item.CountryCode,
                item.Street,
                item.Neighborhood,
                item.City,
                item.District,
                item.PostalCode,
                item.StreetNumber,
                item.Complement,
                item.Latitude,
                item.Longitude,
                item.Notes,
                item.IsPrimary,
                _currentUser.GetUserId());

            var success = await _domain.CreateAsync(entity, ct);

            if (!success)
            {
                _notify.Add(_localization.GetMessage("Application.Service.ClientAddress.ProcessBulkItems.FailedToCreate", item.Street), 400);
                hasErrors = true;
            }
        }

        return !hasErrors;
    }

    private bool ValidateBulkItem(BulkUploadClientAddressItem item)
    {
        if (string.IsNullOrWhiteSpace(item.Street))
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientAddress.ValidateBulkItem.Street", item.ClientId), 400);
            return false;
        }

        if (string.IsNullOrWhiteSpace(item.City))
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientAddress.ValidateBulkItem.City", item.ClientId), 400);
            return false;
        }

        if (string.IsNullOrWhiteSpace(item.PostalCode))
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientAddress.ValidateBulkItem.PostalCode", item.ClientId), 400);
            return false;
        }

        if (string.IsNullOrWhiteSpace(item.CountryCode))
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientAddress.ValidateBulkItem.CountryCode", item.ClientId), 400);
            return false;
        }

        if (item.ClientId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientAddress.ValidateBulkItem.ClientId", item.ClientId), 400);
            return false;
        }

        if (item.AddressTypeId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientAddress.ValidateBulkItem.AddressTypeId", item.ClientId), 400);
            return false;
        }

        return true;
    }
}
