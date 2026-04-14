using AutoMapper;
using Microsoft.Extensions.Logging;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientIndividual;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientIndividual;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Application.Services.Business;

public class ClientIndividualAppService : IClientIndividualAppService
{
    private readonly IClientIndividualDataRepository _repo;
    private readonly IClientRepository _clientRepository;
    private readonly IClientIndividualDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;
    private readonly ILogger<ClientIndividualAppService> _logger;

    public ClientIndividualAppService(
        IClientIndividualDataRepository repo,
        IClientRepository clientRepository,
        IClientIndividualDomainService domain,
        IMapper mapper,
        INotify notify,
        ILocalizationService localization,
        ICurrentUserService currentUser,
        ILogger<ClientIndividualAppService> logger)
    {
        _repo = repo;
        _clientRepository = clientRepository;
        _domain = domain;
        _mapper = mapper;
        _notify = notify;
        _localization = localization;
        _currentUser = currentUser;
        _logger = logger;
    }

    public async Task<ClientIndividualResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(_currentUser.GetTenantId(), id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientIndividual.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<ClientIndividualResponse>(entity);
    }

    public async Task<ClientIndividualResponse> GetByClientIdAsync(int clientId, CancellationToken ct)
    {
        var entity = await _repo.GetByClientIdAsync(_currentUser.GetTenantId(), clientId, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientIndividual.GetByClientId.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<ClientIndividualResponse>(entity);
    }

    public async Task<ClientIndividualResponse> GetByDocumentAsync(string documentType, string documentNumber, CancellationToken ct)
    {
        var entity = await _repo.GetByDocumentAsync(_currentUser.GetTenantId(), documentType, documentNumber, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientIndividual.GetByDocument.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<ClientIndividualResponse>(entity);
    }

    public async Task<IEnumerable<ClientIndividualResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(_currentUser.GetTenantId(), ct);
        return _mapper.Map<IEnumerable<ClientIndividualResponse>>(entities);
    }

    public async Task<IEnumerable<ClientIndividualResponse>> GetActiveAsync(CancellationToken ct)
    {
        var entities = await _repo.GetActiveAsync(_currentUser.GetTenantId(), ct);
        return _mapper.Map<IEnumerable<ClientIndividualResponse>>(entities);
    }

    public async Task<ListPageResponse<ClientIndividualResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.IsActive, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(_currentUser.GetTenantId(), filter, ct);
        return _mapper.Map<ListPageResponse<ClientIndividualResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateClientIndividualRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var client = await _clientRepository.GetAggregateForUpdateAsync(tenantId, request.ClientId, ct);
        if (client == null || client.IsDeleted || !client.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientIndividual.Create.ResourceNotFound"), 410);
            return false;
        }

        if (await _repo.ExistsByClientIdAsync(tenantId, request.ClientId, ct))
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientIndividual.Create.ClientAlreadyHasIndividualData"), 400);
            return false;
        }

        if (!string.IsNullOrWhiteSpace(request.DocumentNumber) &&
            await _repo.ExistsByDocumentAsync(tenantId, request.DocumentType, request.DocumentNumber, ct))
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientIndividual.Create.DocumentAlreadyExists"), 400);
            return false;
        }

        var entity = new ClientIndividualEntity(
            tenantId,
            request.ClientId,
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            request.CellPhoneNumber,
            request.IsWhatsapp,
            request.Email,
            request.BirthDate,
            request.Gender,
            request.DocumentType,
            request.DocumentNumber,
            request.Nationality,
            _currentUser.GetUserId());

        client.SetIndividual(entity);
        return await _clientRepository.UpdateAsync(client, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateClientIndividualRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var entity = await _repo.GetByIdAsync(tenantId, id, ct);

        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientIndividual.Update.ResourceNotFound"), 410);
            return false;
        }

        var client = await _clientRepository.GetAggregateForUpdateAsync(tenantId, entity.ClientId, ct);
        if (client?.Individual == null || client.Individual.Id != id)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientIndividual.Update.ResourceNotFound"), 410);
            return false;
        }

        if (!string.IsNullOrWhiteSpace(request.DocumentNumber) &&
            await _repo.ExistsByDocumentAsync(tenantId, request.DocumentType, request.DocumentNumber, id, ct))
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientIndividual.Update.DocumentAlreadyExists"), 400);
            return false;
        }

        client.Individual.Update(
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            request.CellPhoneNumber,
            request.IsWhatsapp,
            request.Email,
            request.BirthDate,
            request.Gender,
            request.DocumentType,
            request.DocumentNumber,
            request.Nationality,
            _currentUser.GetUserId());

        return await _clientRepository.UpdateAsync(client, ct);
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(_currentUser.GetTenantId(), id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientIndividual.Activate.ResourceNotFound"), 410);
            return false;
        }

        var client = await _clientRepository.GetAggregateForUpdateAsync(_currentUser.GetTenantId(), entity.ClientId, ct);
        if (client?.Individual == null || client.Individual.Id != id)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientIndividual.Activate.ResourceNotFound"), 410);
            return false;
        }

        client.Individual.Activate(_currentUser.GetUserId());
        return await _clientRepository.UpdateAsync(client, ct);
    }

    public async Task<bool> DeactivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(_currentUser.GetTenantId(), id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientIndividual.Deactivate.ResourceNotFound"), 410);
            return false;
        }

        var client = await _clientRepository.GetAggregateForUpdateAsync(_currentUser.GetTenantId(), entity.ClientId, ct);
        if (client?.Individual == null || client.Individual.Id != id)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientIndividual.Deactivate.ResourceNotFound"), 410);
            return false;
        }

        client.Individual.Deactivate(_currentUser.GetUserId());
        return await _clientRepository.UpdateAsync(client, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(_currentUser.GetTenantId(), id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientIndividual.Delete.ResourceNotFound"), 410);
            return false;
        }

        var client = await _clientRepository.GetAggregateForUpdateAsync(_currentUser.GetTenantId(), entity.ClientId, ct);
        if (client?.Individual == null || client.Individual.Id != id)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientIndividual.Delete.ResourceNotFound"), 410);
            return false;
        }

        client.Individual.Delete(_currentUser.GetUserId());
        return await _clientRepository.UpdateAsync(client, ct);
    }
}

