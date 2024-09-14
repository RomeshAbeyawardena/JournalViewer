using JournalViewer.Domain;
using Microsoft.EntityFrameworkCore;

namespace JournalViewer.Infrastructure;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

public interface IRepository<T>
{
    IUnitOfWork UnitOfWork { get; }
    Task<T> Upsert(T entity, CancellationToken cancellationToken,
        Func<T, T, Task<bool>>? updateChallengeAsync = null);
}

public class EntityFrameworkRepositoryBase<TDbContext, T>(TDbContext context) : IRepository<T>
    where TDbContext : DbContext, IUnitOfWork
    where T : class
{
    protected Lazy<DbSet<T>> Entity => new(context.Set<T>);
    public IUnitOfWork UnitOfWork => context;

    public async Task<T> Upsert(T entity, CancellationToken cancellationToken,
        Func<T, T, Task<bool>>? updateChallengeAsync = null)
    {
        if(entity is IIdentifier identifier)
        {
            T? foundEntity;
            if (identifier.Id.HasValue &&
                (foundEntity = await Entity.Value.FindAsync([identifier.Id], cancellationToken)) != null
                && (updateChallengeAsync == null || await updateChallengeAsync(foundEntity, entity)))
            {
                Entity.Value.Update(entity);
                return entity;
            }

            await Entity.Value.AddAsync(entity, cancellationToken);
        }

        throw new InvalidOperationException("Unable to determine state of entity, it does inherit from IIdentifier");
    }
}

