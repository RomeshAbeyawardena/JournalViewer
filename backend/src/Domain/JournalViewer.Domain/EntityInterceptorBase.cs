namespace JournalViewer.Domain;

public abstract class EntityInterceptorBase<TContext, TEntity> : IEntityInterceptor<TContext, TEntity>
{
    public Subject Subject { get; set; }
    public abstract Task<bool> CanIntercept(Subject subject, TContext context, TEntity entity, CancellationToken cancellationToken);
    public abstract Task Intercept(Subject subject, TContext context,
        TEntity entity, CancellationToken cancellationToken);

    Task<bool> IEntityInterceptor.CanIntercept(Subject subject, object context, 
        object entity, CancellationToken cancellationToken)
    {
        return CanIntercept(subject, (TContext)context, (TEntity)entity, cancellationToken);
    }

    Task IEntityInterceptor.Intercept(Subject subject, object context, 
        object entity, CancellationToken cancellationToken)
    {
        return Intercept(subject, (TContext)context, (TEntity)entity, cancellationToken);
    }
}