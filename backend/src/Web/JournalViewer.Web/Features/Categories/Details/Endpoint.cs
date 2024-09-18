using JournalViewer.Domain.Features.Categories.Queries;
using MediatR;

namespace JournalViewer.Web.Features.Categories.Details;

public static class Endpoint
{
    public static async Task<IResult> GetDetails(IMediator mediator, Guid id,
        CancellationToken cancellationToken)
    {
        var category = await mediator.Send(new GetCategory { 
            CategoryId = id
        }, cancellationToken);

        if (category == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(category);
    }
}
