using System.ComponentModel;

namespace JournalViewer.Domain;

public interface IPagedRequest
{
    int? Take { get; set; }
    int? Skip { get; set; }
    string? OrderBy { get; set; }
    ListSortDirection SortDirection { get; set; }
}
