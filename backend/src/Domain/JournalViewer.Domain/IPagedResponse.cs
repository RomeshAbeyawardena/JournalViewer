namespace JournalViewer.Domain;

public interface IPagedList<T> : IEnumerable<T>
{
    int Count { get; }
    int TotalCount { get; }
    int PageSize { get; }
}

public interface IPagedResponse<T> : IPagedList<T>, IQueryable<T>
{
    
}
