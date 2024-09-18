namespace JournalViewer.Domain;

public interface IPagedEnumerable<T> : IEnumerable<T>
{
    int Count { get; }
    int TotalCount { get; }
    int PageSize { get; }
}


public interface IPagedList<T> :  IPagedEnumerable<T>
{
    IPagedList<TDestination> ProjectTo<TDestination>(Func<T, TDestination> projection, CancellationToken cancellationToken);
    
}

public interface IPagedResponse<T> : IPagedEnumerable<T>, IQueryable<T>
{
    internal IPagedRequest PagedRequest { get; }
}
