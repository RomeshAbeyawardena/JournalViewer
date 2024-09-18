using JournalViewer.Infrastructure;
using System.Collections;
using System.Data.Entity;

namespace JournalViewer.Domain;

public interface IAsyncPagedList<T> : IPagedList<T>
{
    ValueTask<IPagedList<T>> GetResultAsync();
}

public class AsyncPagedList<T>(IPagedResponse<T> response, 
    CancellationToken? cancellationToken = null) : IAsyncPagedList<T>
{
    private readonly Lazy<ValueTask<List<T>>> _list = new(async () =>
    {
        return await response.ToListAsync(cancellationToken
            .GetValueOrDefault(CancellationToken.None));
    });

    public IPagedList<TDestination> ProjectTo<TDestination>(Func<T, TDestination> projection, CancellationToken cancellationToken)
    {
        var compiledResponse = new CompiledPagedResponse<TDestination>(TotalCount, response.Select(projection), response.PagedRequest);

        return new AsyncPagedList<TDestination>(compiledResponse, cancellationToken);
    }

    public int Count => _list.Value.GetAwaiter().GetResult().Count;
    public int TotalCount => response.TotalCount;
    public int PageSize => response.PageSize;

    public async ValueTask<IPagedList<T>> GetResultAsync()
    {
        await _list.Value;
        return this;
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return _list.Value.GetAwaiter().GetResult().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _list.Value.GetAwaiter().GetResult().GetEnumerator();
    }
}
