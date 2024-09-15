namespace JournalViewer.Domain;

public interface IPagedResponse
{
    int Count { get; }
    int TotalCount { get; }
    int PageSize { get; }
}
