using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Services.Business;

public class ConsentOriginTypeDomainService : IConsentOriginTypeDomainService
{
    private readonly IConsentOriginTypeDataRepository _repo;
    private readonly IEntityDomainValidator<ConsentOriginTypeEntity> _validator;
    private readonly INotify _notify;

    public ConsentOriginTypeDomainService(
        IConsentOriginTypeDataRepository repo,
        IEntityDomainValidator<ConsentOriginTypeEntity> validator,
        INotify notify)
    {
        _repo = repo;
        _validator = validator;
        _notify = notify;
    }

    public async Task<ConsentOriginTypeEntity> GetByIdAsync(int id, CancellationToken ct)
        => await _repo.GetByIdAsync(id, ct);

    public async Task<IEnumerable<ConsentOriginTypeEntity>> GetAllAsync(CancellationToken ct)
        => await _repo.GetAllAsync(ct);

    public async Task<ListPage<ConsentOriginTypeEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
        => await _repo.GetPagedAsync(request, ct);

    public async Task<bool> ExistsByIdAsync(int id, CancellationToken ct)
        => await _repo.ExistsByIdAsync(id, ct);

    public async Task<bool> CreateAsync(ConsentOriginTypeEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForCreateAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                _notify.Add(error.ErrorMessage, 400);
            return false;
        }
        await _repo.AddAsync(entity, ct);
        return true;
    }

    public async Task<bool> UpdateAsync(ConsentOriginTypeEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForUpdateAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                _notify.Add(error.ErrorMessage, 400);
            return false;
        }
        await _repo.UpdateAsync(entity, ct);
        return true;
    }

    public async Task<bool> ActivateAsync(ConsentOriginTypeEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForActivateAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                _notify.Add(error.ErrorMessage, 400);
            return false;
        }
        await _repo.UpdateAsync(entity, ct);
        return true;
    }

    public async Task<bool> DeactivateAsync(ConsentOriginTypeEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForDeactivateAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                _notify.Add(error.ErrorMessage, 400);
            return false;
        }
        await _repo.UpdateAsync(entity, ct);
        return true;
    }

    public async Task<bool> DeleteAsync(ConsentOriginTypeEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForDeleteAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                _notify.Add(error.ErrorMessage, 400);
            return false;
        }
        await _repo.UpdateAsync(entity, ct);
        return true;
    }
}
