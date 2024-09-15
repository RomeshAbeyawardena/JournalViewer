using JournalViewer.Domain;
using LinqKit;
using System.Linq;
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

    public static IQueryable<T> AsPaged<T>(this IQueryable<T> query, IPagedRequest request)
    {
        if(request.Skip.HasValue)
        {
            query = query.Skip(request.Skip.Value);
        }

        if (request.Take.HasValue)
        {
            query = query.Skip(request.Take.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.OrderBy))
        {
            switch (request.SortDirection)
            {
                case System.ComponentModel.ListSortDirection.Ascending:
                    return query.OrderBy(request.OrderBy);
                case System.ComponentModel.ListSortDirection.Descending:
                    return query.OrderByDescending(request.OrderBy);
            }
        }

        return query;
    }
}
