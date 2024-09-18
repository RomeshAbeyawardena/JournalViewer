using JournalViewer.Domain;
using System.Linq.Expressions;

namespace JournalViewer.Infrastructure;

public static class QueryableExtensions
{
    private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(parameter, propertyName);
        var propAsObject = Expression.Convert(property, typeof(object));

        return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
    }

    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
    {
        return source.OrderBy(ToLambda<T>(propertyName));
    }

    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
    {
        return source.OrderByDescending(ToLambda<T>(propertyName));
    }

    public static IPagedResponse<T> AsPaged<T>(this IQueryable<T> query, IPagedRequest request)
    {
        return new PagedResponse<T>(query, request);
    }

    public static async Task<IPagedList<T>> ToPagedList<T>(this IPagedResponse<T> response, CancellationToken cancellationToken)
    {
        return await new AsyncPagedList<T>(response, cancellationToken)
            .GetResultAsync();
    }
}
