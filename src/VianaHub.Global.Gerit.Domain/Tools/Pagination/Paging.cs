namespace VianaHub.Global.Gerit.Domain.Tools.Pagination;

public class Paging : Order
{
    private const int _maxPageSize = 1000;
    private const int _minPageSize = 10;
    private int? pageNumber;
    private int? pageSize;

    public int? PageNumber
    {
        get { return pageNumber == null || pageNumber <= 0 ? 1 : pageNumber; }
        set { pageNumber = value; }
    }
    public int? PageSize
    {
        get
        {
            if (pageSize == null || pageSize <= 0)
                return _minPageSize;

            return pageSize > _maxPageSize ? _maxPageSize : pageSize;
        }
        set { pageSize = value; }
    }

    public static int MaxPageSize() => _maxPageSize;
    public static int MinPageSize() => _minPageSize;
}
