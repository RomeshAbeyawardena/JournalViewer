using JournalViewer.Domain;
using JournalViewer.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace JournalViewer.Infrastructure.SqlServer.Interceptors;

internal class AddEntityToOutboxOnSaveInterceptor<TEntity>(
    ILogger<AddEntityToOutboxOnSaveInterceptor<TEntity>> logger, 
    TimeProvider timeProvider) : EntityInterceptorBase<JournalViewDbContext, EntityEntry<TEntity>>
    where TEntity : class
{
    private NotificationType? GetNotificationType(EntityState entityState)
    {
        return entityState switch
        {
            EntityState.Added => (NotificationType?)NotificationType.Add,
            EntityState.Modified => (NotificationType?)NotificationType.Update,
            EntityState.Deleted => (NotificationType?)NotificationType.Delete,
            _ => null,
        };
    }

    private static void ConditionalLogTrace(ILogger logger, Action<ILogger> logAction)
    {
        if (logger.IsEnabled(LogLevel.Trace))
        {
            logAction(logger);
        }
    }

    public override Task<bool> CanIntercept(Subject subject, JournalViewDbContext context,
        EntityEntry<TEntity> entity, CancellationToken cancellationToken)
    {
        return Task.FromResult(
            subject == Subject.OnSave &&
            entity.Entity.IsNotifiable(out var notifiableEntity)
                    && notifiableEntity != null);
    }

    public override async Task Intercept(Subject subject, JournalViewDbContext context,
        EntityEntry<TEntity> entity, CancellationToken cancellationToken)
    {
        if(!entity.Entity.IsNotifiable(out var notifiableEntity)
                    || notifiableEntity == null)
        {
            ConditionalLogTrace(logger, logger =>
                logger.LogTrace("Unable to update outbox for entity {name}: Is not the correct type",
                 entity.Metadata.Name));
            return;
        }

        var notificationType = GetNotificationType(entity.State);

        if (!notificationType.HasValue)
        {
            ConditionalLogTrace(logger, logger =>
                logger.LogTrace("Unable to update outbox for entity {name}: Is not the correct notification type",
                 entity.Metadata.Name));
            return;
        }

        var keyValue = entity.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue;

        if (keyValue == null)
        {
            ConditionalLogTrace(logger, logger =>
            logger.LogTrace("Unable to update outbox for entity {name}: Does not have a valid primary key", entity.Metadata.Name));
            return;
        }

        await context.OutboxEntries.AddAsync(new OutboxEntry
        {
            Subject = entity.Metadata.Name,
            EntityId = notifiableEntity.GetKey(entity.Entity)
                        ?? keyValue?.ToString() ?? string.Empty,
            Payload = await notifiableEntity
                                .PrepareNotificationAsync(entity.Entity, notificationType.Value,
                    cancellationToken),
            NotificationType = notificationType.Value,
            Created = timeProvider.GetUtcNow()
        }, cancellationToken);
    }
}
