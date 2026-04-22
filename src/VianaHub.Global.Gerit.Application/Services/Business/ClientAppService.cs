using AutoMapper;
using Azure.Core;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Globalization;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Client;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.Client;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Application.Interfaces.Common;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Enums;
using VianaHub.Global.Gerit.Domain.Helpers;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Application.Services.Business;

public class ClientAppService : IClientAppService
{
    private int UserId { get; set; }
    private int TenantId { get; set; }
    private readonly IClientRepository _repo;
    private readonly IClientDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;
    private readonly IFileValidationService _fileValidation;
    private readonly ILogger<ClientAppService> _logger;


    public ClientAppService(
        IClientRepository repo,
        IClientDomainService domain,
        IMapper mapper,
        INotify notify,
        ILocalizationService localization,
        ICurrentUserService currentUser,
        IFileValidationService fileValidation,
        ILogger<ClientAppService> logger)
    {
        _repo = repo;
        _domain = domain;
        _mapper = mapper;
        _notify = notify;
        _localization = localization;
        _currentUser = currentUser;
        _fileValidation = fileValidation;
        _logger = logger;
        UserId = _currentUser.GetUserId();
        TenantId = _currentUser.GetTenantId();
    }

    public async Task<IEnumerable<ClientResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<ClientResponse>>(entities);
    }

    public async Task<ClientDetailResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Client.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<ClientDetailResponse>(entity);
    }

    public async Task<ListPageResponse<ClientResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.IsActive, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<ClientResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateClientRequest request, CancellationToken ct)
    {
        var client = new ClientEntity(TenantId, (ClientType)request.ClientType, (OriginType)request.OriginType, request.UrlImage, request.Note, UserId);

        switch (client.ClientType)
        {
            case ClientType.PessoaSingular:
            case ClientType.RecibosVerdes:
            case ClientType.Freelancer:
                client.AddIndividual(new ClientIndividualEntity(TenantId, request.Individual.FirstName, request.Individual.LastName, request.Individual.PhoneNumber, request.Individual.CellPhoneNumber, request.Individual.IsWhatsapp, request.Individual.Email, request.Individual.BirthDate, request.Individual.Gender, request.Individual.DocumentType, request.Individual.DocumentNumber, request.Individual.Nationality, UserId));
                break;
            case ClientType.PessoaJuridica:
            case ClientType.SociedadeUnipessoalQuotas:
                client.AddCompany(new ClientCompanyEntity(TenantId, request.Company.LegalName, request.Company.TradeName, request.Company.PhoneNumber, request.Company.CellPhoneNumber, request.Company.IsWhatsapp, request.Company.Email, request.Company.Site, request.Company.CompanyRegistration, request.Company.CAE, request.Company.NumberOfEmployee, request.Company.LegalRepresentative, UserId));
                break;
            default:
                break;
        }

        return await _domain.CreateAsync(client, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateClientRequest request, CancellationToken ct)
    {
        var client = await _repo.GetByIdAsync(id, ct);

        if (client == null || client.IsDeleted || !client.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Client.Update.ResourceNotFound"), 410);
            return false;
        }

        client.Update((ClientType)request.ClientType, (OriginType)request.OriginType, request.UrlImage, request.Note, UserId);

        switch ((ClientType)request.ClientType)
        {
            case ClientType.PessoaSingular:
            case ClientType.RecibosVerdes:
            case ClientType.Freelancer:
                client.UpdateIndividual(request.Individual.FirstName, request.Individual.LastName, request.Individual.PhoneNumber, request.Individual.CellPhoneNumber, request.Individual.IsWhatsapp, request.Individual.Email, request.Individual.BirthDate, request.Individual.Gender, request.Individual.DocumentType, request.Individual.DocumentNumber, request.Individual.Nationality, UserId);
                break;
            case ClientType.PessoaJuridica:
            case ClientType.SociedadeUnipessoalQuotas:
                client.UpdateCompany(request.Company.LegalName, request.Company.TradeName, request.Company.PhoneNumber, request.Company.CellPhoneNumber, request.Company.IsWhatsapp, request.Company.Email, request.Company.Site,request.Company.CompanyRegistration, request.Company.CAE, request.Company.NumberOfEmployee, request.Company.LegalRepresentative, UserId);
                break;
            default:
                break;
        }

        return await _domain.UpdateAsync(client, ct);
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var client = await _repo.GetByIdAsync(id, ct);
        if (client == null || client.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Client.Activate.ResourceNotFound"), 410);
            return false;
        }

        client.Activate(client.ClientType, UserId);
        return await _domain.ActivateAsync(client, ct);
    }

    public async Task<bool> DeactivateAsync(int id, CancellationToken ct)
    {
        var client = await _repo.GetByIdAsync(id, ct);
        if (client == null || client.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Client.Deactivate.ResourceNotFound"), 410);
            return false;
        }

        client.Deactivate(client.ClientType, UserId);
        return await _domain.ActivateAsync(client, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var client = await _repo.GetByIdAsync(id, ct);
        if (client == null || client.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Client.Delete.ResourceNotFound"), 410);
            return false;
        }

        client.Delete(client.ClientType, UserId);
        return await _domain.DeleteAsync(client, ct);
    }

    public async Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct)
    {
        // Valida arquivo usando servi�o centralizado
        if (!_fileValidation.ValidateFile(file))
            return false;

        // L� itens do CSV
        var items = ReadCsvFile(file);
        if (items == null)
            return false;

        if (!items.Any())
        {
            _notify.Add(_localization.GetMessage("Application.Service.Client.BulkUpload.EmptyFile"), 400);
            return false;
        }

        // Processa cada item
        return await ProcessBulkItemsAsync(items, ct);
    }

    private List<BulkUploadClientItem> ReadCsvFile(IFormFile file)
    {
        try
        {
            // Cria StreamReader com encoding UTF-8 for�ado
            using var reader = file.OpenReadStream().CreateUtf8StreamReader();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";", // CSV usa ponto e v�rgula como delimitador
                MissingFieldFound = null,
                HeaderValidated = null,
                TrimOptions = TrimOptions.Trim,
                BadDataFound = null // Ignora linhas mal formatadas ao inv�s de lan�ar exce��o
            };

            using var csv = new CsvReader(reader, config);
            var records = new List<BulkUploadClientItem>();

            csv.Read();
            csv.ReadHeader();

            int rowCount = 0;
            int maxRows = DomainExtensions.GetMaxCsvRows();

            while (csv.Read() && rowCount < maxRows)
            {
                try
                {
                    var record = csv.GetRecord<BulkUploadClientItem>();
                    if (record != null)
                    {
                        records.Add(record);
                    }
                    rowCount++;
                }
                catch (CsvHelperException ex)
                {
                    // Log linha com erro mas continua processamento
                    _logger.LogWarning(ex, "Erro ao processar linha {RowNumber} do CSV de Clients", rowCount + 2);
                    _notify.Add(_localization.GetMessage("Application.Service.Client.ReadCsvFile.CsvHelperException", rowCount + 2), 400);
                    rowCount++;
                    continue;
                }
            }

            if (rowCount >= maxRows)
            {
                _notify.Add(_localization.GetMessage("Application.Service.Client.ReadCsvFile.MaxRows", maxRows), 400);
                return null;
            }

            return records;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao ler arquivo CSV de Clients: {Message}", ex.Message);
            _notify.Add(_localization.GetMessage("Application.Service.Client.ReadCsvFile.Exception"), 400);
            return null;
        }
    }

    private async Task<bool> ProcessBulkItemsAsync(List<BulkUploadClientItem> items, CancellationToken ct)
    {
        var hasErrors = false;
        var tenantId = _currentUser.GetTenantId();

        foreach (var item in items)
        {
            // Valida campos obrigat�rios
            if (!ValidateBulkItem(item))
            {
                hasErrors = true;
                continue;
            }

            // Verifica duplicidade
            //var exists = await _repo.ExistsByEmailAsync(tenantId, item.Email, ct);
            //if (exists)
            //{
            //    _notify.Add(_localization.GetMessage("Application.Service.Client.ProcessBulkItems.ExistsByEmail", item.Email), 400);
            //    hasErrors = true;
            //    continue;
            //}

            // Cria a entidade
            var entity = new ClientEntity(_currentUser.GetTenantId(), (ClientType)item.ClientType, (OriginType)item.OriginType, item.UrlImage, item.Note, _currentUser.GetUserId());

            // Tenta criar no dom�nio
            var success = await _domain.CreateAsync(entity, ct);

            if (!success)
            {
                _notify.Add(_localization.GetMessage("Application.Service.Client.ProcessBulkItems.FailedToCreate"), 400);
                hasErrors = true;
            }
        }

        return !hasErrors;
    }

    private bool ValidateBulkItem(BulkUploadClientItem item)
    {
        if (item.ClientType <=0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Client.ValidateBulkItem.ClientType"), 400);
            return false;
        }

        if (item.OriginType <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Client.ValidateBulkItem.OriginType"), 400);
            return false;
        }

        return true;
    }
}


