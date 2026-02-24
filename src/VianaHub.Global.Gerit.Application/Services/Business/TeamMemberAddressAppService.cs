using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.TeamMemberAddress;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.TeamMemberAddress;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Application.Interfaces.Common;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Helpers;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Application.Services.Business;

public class TeamMemberAddressAppService : ITeamMemberAddressAppService
{
    private readonly ITeamMemberAddressDataRepository _repo;
    private readonly ITeamMemberAddressDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;
    private readonly IFileValidationService _fileValidation;

    public TeamMemberAddressAppService(
        ITeamMemberAddressDataRepository repo,
        ITeamMemberAddressDomainService domain,
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

    public async Task<IEnumerable<TeamMemberAddressResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<TeamMemberAddressResponse>>(entities);
    }

    public async Task<TeamMemberAddressResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberAddress.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<TeamMemberAddressResponse>(entity);
    }

    public async Task<ListPageResponse<TeamMemberAddressResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<TeamMemberAddressResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateTeamMemberAddressRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var exists = await _repo.ExistsByTeamMemberAndAddressAsync(tenantId, request.TeamMemberId, request.Street, request.City, request.PostalCode, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberAddress.Create.ResourceAlreadyExists"), 409);
            return false;
        }

        var entity = new TeamMemberAddressEntity(
            tenantId,
            request.TeamMemberId,
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

    public async Task<bool> UpdateAsync(int id, UpdateTeamMemberAddressRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberAddress.Update.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberAddress.Activate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberAddress.Deactivate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberAddress.Delete.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberAddress.BulkUpload.EmptyFile"), 400);
            return false;
        }

        return await ProcessBulkItemsAsync(items, ct);
    }

    private List<BulkUploadTeamMemberAddressItem> ReadCsvFile(IFormFile file)
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
            var records = new List<BulkUploadTeamMemberAddressItem>();

            csv.Read();
            csv.ReadHeader();

            int rowCount = 0;
            int maxRows = DomainExtensions.GetMaxCsvRows();

            while (csv.Read() && rowCount < maxRows)
            {
                try
                {
                    var record = csv.GetRecord<BulkUploadTeamMemberAddressItem>();
                    if (record != null)
                    {
                        record.Street = record.Street?.SanitizeCsvInput().NormalizeUtf8();
                        record.City = record.City?.SanitizeCsvInput().NormalizeUtf8();
                        record.PostalCode = record.PostalCode?.SanitizeCsvInput().NormalizeUtf8();
                        record.District = record.District?.SanitizeCsvInput().NormalizeUtf8();
                        record.CountryCode = record.CountryCode?.SanitizeCsvInput().NormalizeUtf8();

                        if (!string.IsNullOrEmpty(record.Street) && !record.Street.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberAddress.ReadCsvFile.Street.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        if (!string.IsNullOrEmpty(record.City) && !record.City.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberAddress.ReadCsvFile.City.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        if (!string.IsNullOrEmpty(record.PostalCode) && !record.PostalCode.IsSafeCsvValue())
                        {
                            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberAddress.ReadCsvFile.PostalCode.IsSafeCsvValue", rowCount + 2), 400);
                            continue;
                        }

                        records.Add(record);
                    }
                    rowCount++;
                }
                catch (CsvHelperException)
                {
                    _notify.Add(_localization.GetMessage("Application.Service.TeamMemberAddress.ReadCsvFile.CsvHelperException", rowCount + 2), 400);
                    rowCount++;
                    continue;
                }
            }

            if (rowCount >= maxRows)
            {
                _notify.Add(_localization.GetMessage("Application.Service.TeamMemberAddress.ReadCsvFile.MaxRows", maxRows), 400);
                return null;
            }

            return records;
        }
        catch (Exception)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberAddress.ReadCsvFile.Exception"), 400);
            return null;
        }
    }

    private async Task<bool> ProcessBulkItemsAsync(List<BulkUploadTeamMemberAddressItem> items, CancellationToken ct)
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

            var exists = await _repo.ExistsByTeamMemberAndAddressAsync(tenantId, item.TeamMemberId, item.Street, item.City, item.PostalCode, ct);
            if (exists)
            {
                _notify.Add(_localization.GetMessage("Application.Service.TeamMemberAddress.ProcessBulkItems.ExistsByAddress", item.Street), 400);
                hasErrors = true;
                continue;
            }

            var entity = new TeamMemberAddressEntity(
                tenantId,
                item.TeamMemberId,
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
                _notify.Add(_localization.GetMessage("Application.Service.TeamMemberAddress.ProcessBulkItems.FailedToCreate", item.Street), 400);
                hasErrors = true;
            }
        }

        return !hasErrors;
    }

    private bool ValidateBulkItem(BulkUploadTeamMemberAddressItem item)
    {
        if (string.IsNullOrWhiteSpace(item.Street))
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberAddress.ValidateBulkItem.Street", item.TeamMemberId), 400);
            return false;
        }

        if (string.IsNullOrWhiteSpace(item.City))
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberAddress.ValidateBulkItem.City", item.TeamMemberId), 400);
            return false;
        }

        if (string.IsNullOrWhiteSpace(item.PostalCode))
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberAddress.ValidateBulkItem.PostalCode", item.TeamMemberId), 400);
            return false;
        }

        if (string.IsNullOrWhiteSpace(item.CountryCode))
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberAddress.ValidateBulkItem.CountryCode", item.TeamMemberId), 400);
            return false;
        }

        if (item.TeamMemberId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TeamMemberAddress.ValidateBulkItem.TeamMemberId", item.TeamMemberId), 400);
            return false;
        }

        return true;
    }
}
