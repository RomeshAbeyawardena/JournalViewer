using JournalViewer.Domain;
using JournalViewer.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace JournalViewer.Infrastructure.SqlServer;

public class JournalViewDbContext : DbContext
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

    public DbSet<Element> Elements { get; set; }
    public DbSet<OutboxEntry> OutboxEntries { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var logger = this.GetService<ILogger<JournalViewDbContext>>();

        var timeProvider = this.GetService<TimeProvider>();
        var outboxEntryList = new List<OutboxEntry>();
        foreach (var entry in ChangeTracker.Entries())
        {
            try
            {
                if (!entry.Entity.IsNotifiable(out var notifiableEntity)
                    || notifiableEntity == null)
                {
                    logger.LogTrace("Unable to update outbox for entity {name}: Is not notifiable",
                        entry.Metadata.Name);
                    continue;
                }

                var notificationType = GetNotificationType(entry.State);

                if (!notificationType.HasValue)
                {
                    logger.LogTrace("Unable to update outbox for entity {name}: Is not the correct type",
                         entry.Metadata.Name);
                    continue;
                }

                var keyValue = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue;

                if (keyValue == null)
                {
                    logger.LogTrace("Unable to update outbox for entity {name}: Does not have a valid primary key", entry.Metadata.Name);
                    continue;
                }

                outboxEntryList.Add(new OutboxEntry
                {
                    EntityId = notifiableEntity.GetKey(entry)
                        ?? keyValue?.ToString() ?? string.Empty,
                    Payload = await notifiableEntity
                                .PrepareNotificationAsync(entry, notificationType.Value,
                    cancellationToken),
                    NotificationType = notificationType.Value,
                    Created = timeProvider.GetUtcNow()
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to update outbox for entity {name}", entry.Metadata.Name);
            }
        }
        // Add all outbox entries at once
        if (outboxEntryList.Count > 0)
        {
            OutboxEntries.AddRange(outboxEntryList);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
