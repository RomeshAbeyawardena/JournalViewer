using JournalViewer.Domain;
using JournalViewer.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace JournalViewer.Infrastructure.SqlServer;

public class JournalViewDbContext : DbContext
{
    private NotificationType? GetNotificationType(EntityState entityState)
    {
        switch (entityState) {
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
        var timeProvider = this.GetService<TimeProvider>();

        foreach(var entry in ChangeTracker.Entries())
        {

            if(!entry.Entity.IsNotifiable(out var notifiableEntity))
            {
                continue;
            }

            var notificationType = GetNotificationType(entry.State);

            if (!notificationType.HasValue)
            {
                continue;
            }

            OutboxEntries.Add(new OutboxEntry
            {
                EntityId = notifiableEntity.GetKey(entry) ?? string.Empty,
                Payload = await notifiableEntity
                            .PrepareNotificationAsync(entry, notificationType.Value,
                cancellationToken),
                NotificationType = notificationType.Value,
                Created = timeProvider.GetUtcNow()
            });
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
