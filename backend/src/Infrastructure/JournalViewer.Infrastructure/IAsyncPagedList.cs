namespace JournalViewer.Domain;

public interface IAsyncPagedList<T> : IPagedList<T>
{
    ValueTask<IPagedList<T>> GetResultAsync();
}
