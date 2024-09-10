using JournalViewer.Domain;
using JournalViewer.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace JournalViewer.Infrastructure.SqlServer;

public class JournalViewDbContext : DbContext
{
    public DbSet<Element> Elements { get; set; }
    public DbSet<OutboxEntry> OutboxEntries { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entriesExcludingOutbox = ChangeTracker.Entries()
            .Where(e => e.Entity.GetType() != typeof(OutboxEntry) && e.Entity.IsNotifiable())
            .Select(e => e.Entity as INotifiableEntity)
            .Where(e => e != null);

        var timeProvider = this.GetService<TimeProvider>();

        foreach(var entry in entriesExcludingOutbox)
        {
            OutboxEntries.Add(new OutboxEntry
            {
                EntityId = entry.GetKey(entry),
                NotificationType = entry.NotificationType,
                Payload = await entry.PrepareNotificationAsync(entry, entry.NotificationType,
                cancellationToken),
                Created = timeProvider.GetUtcNow()
            });
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
