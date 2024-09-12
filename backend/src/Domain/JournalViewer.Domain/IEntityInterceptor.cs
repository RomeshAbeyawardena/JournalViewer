namespace JournalViewer.Domain;

public enum Subject
{
    OnInsert,
    OnUpdate,
    OnSave
}

public interface IEntityInterceptor
{
    Subject Subject { get; }
    Task<bool> CanIntercept(Subject subject, object context, 
        object entity, CancellationToken cancellationToken);
    Task Intercept(Subject subject, object context, object entity, CancellationToken cancellationToken);
}

public interface IEntityInterceptor<TContext, TEntity> : IEntityInterceptor
{
    IEntityInterceptor<TContext, TEntity> ChangeType<TSourceType>(IEntityInterceptor<TContext, TSourceType> type);
    Task<bool> CanIntercept(Subject subject, TContext context, TEntity entity, CancellationToken cancellationToken);
    Task Intercept(Subject subject, TContext context, 
        TEntity entity, CancellationToken cancellationToken);
}
