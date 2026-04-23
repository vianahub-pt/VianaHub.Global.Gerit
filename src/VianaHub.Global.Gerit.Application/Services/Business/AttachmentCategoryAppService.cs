using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.AttachmentCategory;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.AttachmentCategory;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Application.Interfaces.Common;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Application.Services.Business;

public class AttachmentCategoryAppService : IAttachmentCategoryAppService
{
    private readonly IAttachmentCategoryDataRepository _repo;
    private readonly IAttachmentCategoryDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;

    public AttachmentCategoryAppService(
        IAttachmentCategoryDataRepository repo,
        IAttachmentCategoryDomainService domain,
        IMapper mapper,
        INotify notify,
        ILocalizationService localization,
        ICurrentUserService currentUser)
    {
        _repo = repo;
        _domain = domain;
        _mapper = mapper;
        _notify = notify;
        _localization = localization;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<AttachmentCategoryResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<AttachmentCategoryResponse>>(entities);
    }

    public async Task<IEnumerable<AttachmentCategoryResponse>> GetActiveAsync(CancellationToken ct)
    {
        var entities = await _repo.GetActiveAsync(ct);
        return _mapper.Map<IEnumerable<AttachmentCategoryResponse>>(entities);
    }

    public async Task<AttachmentCategoryResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.AttachmentCategory.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<AttachmentCategoryResponse>(entity);
    }

    public async Task<ListPageResponse<AttachmentCategoryResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.IsActive, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<AttachmentCategoryResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateAttachmentCategoryRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();

        if (await _repo.ExistsByNameAsync(tenantId, request.Name, ct))
        {
            _notify.Add(_localization.GetMessage("Application.Service.AttachmentCategory.Create.NameAlreadyExists"), 409);
            return false;
        }

        var entity = new AttachmentCategoryEntity(
            tenantId,
            request.Name,
            request.Description,
            request.DisplayOrder,
            false,
            _currentUser.GetUserId()
        );

        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateAttachmentCategoryRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.AttachmentCategory.Update.ResourceNotFound"), 410);
            return false;
        }

        var tenantId = _currentUser.GetTenantId();
        if (await _repo.ExistsByNameAsync(tenantId, request.Name, id, ct))
        {
            _notify.Add(_localization.GetMessage("Application.Service.AttachmentCategory.Update.NameAlreadyExists"), 409);
            return false;
        }

        entity.Update(request.Name, request.Description, request.DisplayOrder, _currentUser.GetUserId());

        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.AttachmentCategory.Activate.ResourceNotFound"), 410);
            return false;
        }

        entity.Activate(_currentUser.GetUserId());
        return await _domain.ActivateAsync(entity, ct);
    }

    public async Task<bool> DeactivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.AttachmentCategory.Deactivate.ResourceNotFound"), 410);
            return false;
        }

        entity.Deactivate(_currentUser.GetUserId());
        return await _domain.DeactivateAsync(entity, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.AttachmentCategory.Delete.ResourceNotFound"), 410);
            return false;
        }

        entity.Delete(_currentUser.GetUserId());
        return await _domain.DeleteAsync(entity, ct);
    }
}
