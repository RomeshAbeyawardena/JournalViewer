using System.Collections;
using System.Linq.Expressions;
using JournalViewer.Domain;

namespace JournalViewer.Infrastructure;

public class PagedResponse<T>(IQueryable<T> query, IPagedRequest pagedRequest) : IPagedResponse<T>
{
    private readonly Lazy<IQueryable<T>> _pagedQuery = new(() =>
    {
        var q = query;
        if (pagedRequest.Skip.HasValue)
        {
            q.Skip(pagedRequest.Skip.Value);
        }

        if (pagedRequest.Take.HasValue)
        {
            q = q.Take(pagedRequest.Take.Value);
        }

        if (!string.IsNullOrWhiteSpace(pagedRequest.OrderBy))
        {
            switch (pagedRequest.SortDirection)
            {
                case System.ComponentModel.ListSortDirection.Ascending:
                    return q.OrderBy(pagedRequest.OrderBy);
                case System.ComponentModel.ListSortDirection.Descending:
                    return q.OrderByDescending(pagedRequest.OrderBy);
            }
        }

        return q;
    });

    public int Count => _pagedQuery.Value.Count();
    public int TotalCount => query.Count();
    public int PageSize => pagedRequest.Take.GetValueOrDefault();

    Type IQueryable.ElementType => _pagedQuery.Value.ElementType;
    Expression IQueryable.Expression => _pagedQuery.Value.Expression;
    IQueryProvider IQueryable.Provider => _pagedQuery.Value.Provider;

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return _pagedQuery.Value.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _pagedQuery.Value.GetEnumerator();
    }


    IPagedRequest IPagedResponse<T>.PagedRequest => pagedRequest;
}