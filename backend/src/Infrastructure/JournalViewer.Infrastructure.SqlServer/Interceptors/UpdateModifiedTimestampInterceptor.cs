using JournalViewer.Domain.Bootstrap;
using JournalViewer.Domain.Characteristics;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JournalViewer.Infrastructure.SqlServer.Interceptors;

public class UpdateModifiedTimestampInterceptor<TEntity>(TimeProvider timeProvider)
    : EntityInterceptorBase<JournalViewDbContext, EntityEntry<TEntity>>(Subject.OnUpdate)
    where TEntity : class
{
    public override async Task<bool> CanIntercept(Subject subject, JournalViewDbContext context, EntityEntry<TEntity> entity, CancellationToken cancellationToken)
    {
        return await base.CanIntercept(subject, context, entity, cancellationToken)
            && entity is IModifiedTimestamp;
    }
    public override Task Intercept(Subject subject, JournalViewDbContext context, 
        EntityEntry<TEntity> entity, CancellationToken cancellationToken)
    {
        if(entity is IModifiedTimestamp modified)
        {
            modified.Modified = timeProvider.GetUtcNow();
        }

        return Task.CompletedTask;
    }
}
