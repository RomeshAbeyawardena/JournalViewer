using JournalViewer.Domain;
using JournalViewer.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace JournalViewer.Infrastructure.SqlServer;

public class JournalViewDbContext : DbContext
{
    public DbSet<Element> Elements { get; set; }
    public DbSet<OutboxEntry> OutboxEntries { get; set; }

    public override async ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
    {
        var logger = this.GetService<ILogger<JournalViewDbContext>>();
        var factory = this.GetService<IEntityInterceptorFactory<JournalViewDbContext>>();
        var subject = Subject.OnInsert;
        try
        {
            var interceptor = factory.GetInterceptor<TEntity>(subject);

            if (await interceptor.CanIntercept(subject,
                    this, entity, cancellationToken))
            {
                await interceptor.Intercept(subject, this,
                    entity, cancellationToken);
            }
        }
        catch(Exception exception)
        {
            logger.LogError(exception, "Unable to intercept registered intercepted for entity {name}", typeof(TEntity).Name);
        }

        return await base.AddAsync(entity, cancellationToken);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var logger = this.GetService<ILogger<JournalViewDbContext>>();
        var factory = this.GetService<IEntityInterceptorFactory<JournalViewDbContext>>();
        var subject = Subject.OnSave;
        foreach (var entry in ChangeTracker.Entries())
        {
            try
            {
                var interceptor = factory.GetInterceptor(subject);

                if (await interceptor.CanIntercept(subject, 
                    this, entry, cancellationToken))
                {
                    await interceptor.Intercept(subject, this,
                        entry, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to update outbox for entity {name}", entry.Metadata.Name);
            }
        }
        
        return await base.SaveChangesAsync(cancellationToken);
    }
}
