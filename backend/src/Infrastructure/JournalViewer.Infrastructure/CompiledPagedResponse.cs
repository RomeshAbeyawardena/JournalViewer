using System.Collections;
using System.Linq.Expressions;
using JournalViewer.Domain;

namespace JournalViewer.Infrastructure;

internal class CompiledPagedResponse<T>(int totalCount, 
    IEnumerable<T> query, IPagedRequest pagedRequest) : IPagedResponse<T>
{
    private readonly Lazy<IEnumerable<T>> _query = new(query.ToList);

    public int Count => _query.Value.Count();
    public int TotalCount => totalCount;
    public int PageSize => pagedRequest.Take.GetValueOrDefault();
    public Type ElementType => throw new NotSupportedException();
    public Expression Expression => throw new NotSupportedException();
    public IQueryProvider Provider => throw new NotSupportedException();

    public IEnumerator<T> GetEnumerator()
    {
        return _query.Value.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _query.Value.GetEnumerator();
    }

    IPagedRequest IPagedResponse<T>.PagedRequest => pagedRequest;
}
