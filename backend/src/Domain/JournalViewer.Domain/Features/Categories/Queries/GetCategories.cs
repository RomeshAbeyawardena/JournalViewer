using MediatR;

namespace JournalViewer.Domain.Features.Categories.Queries;

public class GetCategories : IRequest<IEnumerable<Category>>
{
    public required CategoryFilter Filter { get; set; }
}
