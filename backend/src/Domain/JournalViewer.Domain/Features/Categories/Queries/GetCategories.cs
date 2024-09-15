using MediatR;
using System.ComponentModel;

namespace JournalViewer.Domain.Features.Categories.Queries;

public class GetCategories : IPagedRequest, IRequest<IEnumerable<Category>>
{
    public required CategoryFilter Filter { get; set; }
    public string NameContains { get; set; }
    public bool? ShowAll { get; set; }
    public int? Take { get; set; }
    public int? Skip { get; set; }
    public string? OrderBy { get; set; }
    public ListSortDirection SortDirection { get; set; }
}
