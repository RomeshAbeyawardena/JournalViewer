using MediatR;

namespace JournalViewer.Domain.Features.Categories.Queries;

public class GetCategory : IRequest<Category>
{
    public Guid CategoryId { get; set; }
}
