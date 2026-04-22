using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Services.Business;

/// <summary>
/// Serviço de domínio para ClientContact
/// </summary>
public class ClientContactDomainService : IClientContactDomainService
{
    private readonly IClientContactDataRepository _repo;
    private readonly IEntityDomainValidator<ClientContactEntity> _validator;
    private readonly INotify _notify;

    public ClientContactDomainService(
        IClientContactDataRepository repo,
        IEntityDomainValidator<ClientContactEntity> validator,
        INotify notify)
    {
        _repo = repo;
        _validator = validator;
        _notify = notify;
    }

    public async Task<ClientContactEntity> GetByIdAsync(int clientId, int id, CancellationToken ct)
    {
        return await _repo.GetByIdAsync(clientId, id, ct);
    }
    public async Task<IEnumerable<ClientContactEntity>> GetAllAsync(int clientId, CancellationToken ct)
    {
        return await _repo.GetAllAsync(clientId, ct);
    }
    public async Task<ListPage<ClientContactEntity>> GetPagedAsync(int clientId, PagedFilter request, CancellationToken ct)
    {
        return await _repo.GetPagedAsync(clientId, request, ct);
    }
    public async Task<bool> ExistsByClientAndEmailAsync(int clientId, string name, string email, CancellationToken ct)
    {
        return await _repo.ExistsByClientAndEmailAsync(clientId, name, email, ct);
    }

    public async Task<bool> CreateAsync(ClientContactEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForCreateAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _notify.Add(error.ErrorMessage, 400);
            }
            return false;
        }

        return await _repo.AddAsync(entity, ct);
    }
    public async Task<bool> UpdateAsync(ClientContactEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForUpdateAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _notify.Add(error.ErrorMessage, 400);
            }
            return false;
        }

        return await _repo.UpdateAsync(entity, ct);
    }
    public async Task<bool> ActivateAsync(ClientContactEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForActivateAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _notify.Add(error.ErrorMessage, 400);
            }
            return false;
        }

        return await _repo.UpdateAsync(entity, ct);
    }
    public async Task<bool> DeactivateAsync(ClientContactEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForDeactivateAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _notify.Add(error.ErrorMessage, 400);
            }
            return false;
        }

        return await _repo.UpdateAsync(entity, ct);
    }
    public async Task<bool> DeleteAsync(ClientContactEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForDeleteAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _notify.Add(error.ErrorMessage, 400);
            }
            return false;
        }

        return await _repo.UpdateAsync(entity, ct);
    }
}
