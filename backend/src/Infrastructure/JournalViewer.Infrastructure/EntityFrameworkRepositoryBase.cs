using JournalViewer.Domain;
using JournalViewer.Domain.TypeCache;
using Microsoft.EntityFrameworkCore;

namespace JournalViewer.Infrastructure;

public class EntityFrameworkRepositoryBase<TDbContext, TDb, T>(TDbContext context, 
    ITypeCacheProvider typeCacheProvider) : IRepository<T>
    where TDbContext : DbContext, IUnitOfWork
    where TDb : class
{
    protected Lazy<DbSet<TDb>> Entity => new(context.Set<TDb>);
    public IUnitOfWork UnitOfWork => context;

    public async Task<T> Upsert(T entity, CancellationToken cancellationToken,
        Func<TDb, TDb, Task<bool>>? updateChallengeAsync = null)
    {
        if(entity is not IMappable<T> mappable)
        {
            throw new InvalidOperationException();
        }

        var dbEntity = mappable.MapTo<TDb>(entity, typeCacheProvider);

        if (dbEntity is IIdentifier identifier)
        {
            TDb? foundEntity;
            if (identifier.Id.HasValue &&
                (foundEntity = await Entity.Value.FindAsync([identifier.Id], cancellationToken)) != null
                && (updateChallengeAsync == null || await updateChallengeAsync(foundEntity, dbEntity)))
            {
                Entity.Value.Update(dbEntity);
                return entity;
            }

            await Entity.Value.AddAsync(dbEntity, cancellationToken);
        }

        throw new InvalidOperationException("Unable to determine state of entity, it does inherit from IIdentifier");
    }

    public virtual Task<bool> ChallengeUpdate(TDb foundEntry, TDb originalEntry)
    {
        return Task.FromResult(true);
    }

    Task<T> IRepository<T>.Upsert(T entity, CancellationToken cancellationToken)
    {
        return Upsert(entity, cancellationToken, ChallengeUpdate);
    }
}

