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
        switch (entityState)
        {
            case EntityState.Added:
                return NotificationType.Add;
            case EntityState.Modified:
                return NotificationType.Update;
            case EntityState.Deleted:
                return NotificationType.Delete;
            default:
                return null;
        }
    }

    public DbSet<Element> Elements { get; set; }
    public DbSet<OutboxEntry> OutboxEntries { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var logger = this.GetService<ILogger<JournalViewDbContext>>();

        var timeProvider = this.GetService<TimeProvider>();

        foreach (var entry in ChangeTracker.Entries())
        {
            try
            {
                if (!entry.Entity.IsNotifiable(out var notifiableEntity)
                    || notifiableEntity == null)
                {
                    continue;
                }

                var notificationType = GetNotificationType(entry.State);

                if (!notificationType.HasValue)
                {
                    continue;
                }

                var primaryKey = entry.Metadata.FindPrimaryKey();

                if (primaryKey == null)
                {
                    continue;
                }

                var keyValue = entry.Property(primaryKey.Properties.First().Name).CurrentValue;

                OutboxEntries.Add(new OutboxEntry
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
        return await base.SaveChangesAsync(cancellationToken);
    }
}
