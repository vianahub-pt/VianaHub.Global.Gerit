using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.ReadModels;

public class PagedFilter : Paging
{

    public Guid? TenantId { get; set; }
    public string Search { get; set; }
    public PagedFilter(Guid? tenantId, string search, int? pageNumber, int? pageSize, string sortBy, string sortDirection)
    {
        TenantId = tenantId;
        Search = search;
        PageNumber = pageNumber;
        PageSize = pageSize;
        SortBy = sortBy;
        SortDirection = sortDirection;
    }
}
