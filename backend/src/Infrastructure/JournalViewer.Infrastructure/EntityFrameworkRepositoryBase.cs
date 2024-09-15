using JournalViewer.Domain.Bootstrap;
using JournalViewer.Domain.Characteristics;
using JournalViewer.Domain.TypeCache;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace JournalViewer.Infrastructure;

public class EntityFrameworkRepositoryBase<TDbContext, TDb, T>(TDbContext context,
    ITypeCacheProvider typeCacheProvider) : IRepository<T>
    where TDbContext : DbContext, IUnitOfWork
    where TDb : class
{
    private readonly Lazy<DbSet<TDb>> _entity = new(context.Set<TDb>);
    private readonly Lazy<ExpressionStarter<TDb>> _expressionStarter = new(() => {
        var p = PredicateBuilder.New<TDb>();
        p.DefaultExpression = a => true;
        return p;
    });

    protected Lazy<ExpressionStarter<TDb>> Query  => _expressionStarter;
    protected Lazy<DbSet<TDb>> Entity => _entity;

    protected virtual TDb Map(T source)
    {
        throw new InvalidOperationException("Unable to map between domain and entity types");
    }

    protected virtual T Map(TDb source)
    {
        throw new InvalidOperationException("Unable to map between domain and entity types");
    }

    public IUnitOfWork UnitOfWork => context;

    public async Task<T> Upsert(T entity, CancellationToken cancellationToken,
        Func<TDb, TDb, Task<bool>>? updateChallengeAsync = null)
    {
        TDb dbEntity;
        if (entity is IMappable<T> mappable)
        {
            dbEntity = mappable.MapTo<TDb>(entity, typeCacheProvider);
        }
        else
        {
            dbEntity = Map(entity);
        }

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

    public async ValueTask<T?> FindAsync(IEnumerable<object> items, CancellationToken cancellationToken)
    {
        var foundEntity = await Entity.Value.FindAsync(items.ToArray(), cancellationToken);
        if(foundEntity == default)
        {
            return default;
        }

        //try to use domain based mapping before using the potentially overridden members for mapping
        if(foundEntity is not IMappable<TDb> mappable)
        {
            return Map(foundEntity);
        }

        return mappable.MapTo<T>(foundEntity);
    }
}

