namespace JournalViewer.Domain.Extensions;

public static class EnumerableExtensions
{
    public static async Task HandleAsync(this IEnumerable<IEntityInterceptor> entityInterceptors,
        Subject subject, object context, object entity, CancellationToken cancellationToken,
        Action<Exception>? handleError = null)
    {
        foreach (var interceptor in entityInterceptors)
        {
            try
            {
                if(!await interceptor.CanIntercept(subject, context, entity, cancellationToken))
                {
                    return;
                }

                await interceptor.Intercept(subject, context, entity, cancellationToken);
            }
            catch (Exception exception)
            {
                handleError?.Invoke(exception);
            }
        }
    }

    public static async Task HandleAsync<TContext, TEntity>(this IEnumerable<IEntityInterceptor<TContext,TEntity>> entityInterceptors,
        Subject subject, TContext context, TEntity entity, CancellationToken cancellationToken,
        Action<Exception>? handleError = null)
    {
        foreach (var interceptor in entityInterceptors)
        {
            try
            {
                if (!await interceptor.CanIntercept(subject, context, entity, cancellationToken))
                {
                    return;
                }

                await interceptor.Intercept(subject, context, entity, cancellationToken);
            }
            catch (Exception exception)
            {
                handleError?.Invoke(exception);
            }
        }
    }
}
