namespace JournalViewer.Domain;

public interface IRepository<T>
{
    IUnitOfWork UnitOfWork { get; }
    Task<T> Upsert<TExternal>(T entity, CancellationToken cancellationToken,
        Func<TExternal, TExternal, Task<bool>>? updateChallengeAsync = null);
}