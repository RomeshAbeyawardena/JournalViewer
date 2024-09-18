namespace JournalViewer.Domain;

public interface IPagedResponse<T> : IQueryable<T>
{
    int Count { get; }
    int TotalCount { get; }
    int PageSize { get; }
}
