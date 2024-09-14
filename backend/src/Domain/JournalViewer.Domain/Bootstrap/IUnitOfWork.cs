namespace JournalViewer.Domain.Bootstrap;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

