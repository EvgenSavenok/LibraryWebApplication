namespace Application.DataTransferObjects;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
}
