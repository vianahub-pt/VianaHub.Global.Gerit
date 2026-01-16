using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.ReadModels;

public class PagedFilter : Paging
{

    public string Search { get; set; }
    public PagedFilter(string search, int? pageNumber, int? pageSize, string sortBy, string sortDirection)
    {
        Search = search;
        PageNumber = pageNumber;
        PageSize = pageSize;
        SortBy = sortBy;
        SortDirection = sortDirection;
    }
}
