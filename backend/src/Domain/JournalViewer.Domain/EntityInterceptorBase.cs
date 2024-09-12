﻿namespace JournalViewer.Domain;

public abstract class EntityInterceptorBase<TContext, TEntity>(Subject subject) : IEntityInterceptor<TContext, TEntity>
{
    public Subject Subject { get; } = subject;

    public virtual Task<bool> CanIntercept(Subject subject, TContext context, TEntity entity, CancellationToken cancellationToken)
    {
        return Task.FromResult(Subject == subject);
    }

    public virtual IEntityInterceptor<TContext, TEntity> ChangeType<TSourceType>(IEntityInterceptor<TContext, TSourceType> type)
    {
        return this;
    }

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