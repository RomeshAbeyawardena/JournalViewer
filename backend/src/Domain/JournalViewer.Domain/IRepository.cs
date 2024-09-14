namespace JournalViewer.Domain;

public interface IRepository<T>
{
    IUnitOfWork UnitOfWork { get; }
    Task<T> Upsert(T entity, CancellationToken cancellationToken);
    ValueTask<T?> FindAsync(IEnumerable<object> items, CancellationToken cancellationToken);
}