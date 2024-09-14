using JournalViewer.Domain;
using JournalViewer.Domain.Extensions;
using JournalViewer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace JournalViewer.Infrastructure.SqlServer;

public class JournalViewDbContext : DbContext
{
    private void HandleError(Exception exception)
    {
        throw new InvalidOperationException(exception.Message, exception);
    }

    public DbSet<Element> Elements { get; set; }
    public DbSet<OutboxEntry> OutboxEntries { get; set; }

    public override async ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
    {
        var logger = this.GetService<ILogger<JournalViewDbContext>>();
        var factory = this.GetService<IEntityInterceptorFactory<JournalViewDbContext>>();
        var subject = Subject.OnInsert;
        try
        {
            var interceptors = factory.GetInterceptors<TEntity>(subject);
            if (interceptors != null)
            {
                await interceptors.HandleAsync(subject, this, entity, cancellationToken, HandleError);
            }
        }
        catch(Exception exception)
        {
            logger.LogError(exception, "Unable to intercept registered intercepted for entity {name}",
               typeof(TEntity).Name);
        }

        return await base.AddAsync(entity, cancellationToken);
    }

    public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
    {
        var timeProvider = this.GetService<TimeProvider>();
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30), timeProvider);
        var logger = this.GetService<ILogger<JournalViewDbContext>>();
        var factory = this.GetService<IEntityInterceptorFactory<JournalViewDbContext>>();
        var subject = Subject.OnUpdate;
        try
        {
            var interceptor = factory.GetInterceptors<TEntity>(subject);
            interceptor.HandleAsync(subject, this, entity, cancellationTokenSource.Token,
                HandleError).GetAwaiter().GetResult();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unable to intercept registered intercepted for entity {name}", typeof(TEntity).Name);
        }
        return base.Update(entity);
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
                var interceptors = factory.GetInterceptors(subject, entry.GetType());
                await interceptors.HandleAsync(subject, this, entry, cancellationToken,
                    HandleError);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to intercept registered intercepted for entity {name}", entry.Metadata.Name);
            }
        }
        
        return await base.SaveChangesAsync(cancellationToken);
    }
}
