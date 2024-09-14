using JournalViewer.Domain.Bootstrap;
using JournalViewer.Domain.Extensions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JournalViewer.Infrastructure.SqlServer.Interceptors;

public class AddCreatedTimestampInterceptor<TEntity>(TimeProvider timeProvider)
    : EntityInterceptorBase<JournalViewDbContext, EntityEntry<TEntity>>(Subject.OnInsert)
    where TEntity : class
{
    public override Type ChangeType(Type type)
    {
        return typeof(EntityEntry<>).MakeGenericType(type);
    }

    public override async Task<bool> CanIntercept(Subject subject, JournalViewDbContext context, EntityEntry<TEntity> entity, CancellationToken cancellationToken)
    {
        return await base.CanIntercept(subject, context, entity, cancellationToken)
            && entity.Entity.HasCreatedTimestamp(out var createdTimestamp) && createdTimestamp != null;
    }

    public override Task Intercept(Subject subject, JournalViewDbContext context, EntityEntry<TEntity> entity, CancellationToken cancellationToken)
    {
        if(!entity.Entity.HasCreatedTimestamp(out var createdTimestamp) 
            || createdTimestamp == null)
        {
            return Task.CompletedTask;
        }

        createdTimestamp.Created = timeProvider.GetUtcNow();
        return Task.CompletedTask;
    }
}
