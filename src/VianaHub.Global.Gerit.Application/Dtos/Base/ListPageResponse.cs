namespace VianaHub.Global.Gerit.Application.Dtos.Base;

public class ListPageResponse<T>
{
    public IEnumerable<T> Data { get; set; } = new List<T>();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }

    public ListPageResponse()
    {
    }

    public ListPageResponse(IEnumerable<T> data, int pageNumber, int pageSize, int totalItems, int totalPages)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItems = totalItems;
        TotalPages = totalPages;
    }
}
