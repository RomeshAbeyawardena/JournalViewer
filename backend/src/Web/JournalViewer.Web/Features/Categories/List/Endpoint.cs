﻿using JournalViewer.Domain.Features.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JournalViewer.Web.Features.Categories.List;

public static class Endpoint
{
    public static async Task<IResult> GetCategories(
        IMediator mediator,
        [FromQuery] GetCategories query,
        CancellationToken cancellationToken)
    {
        var results = await mediator.Send(query, cancellationToken);

        if (!results.Any())
        {
            return Results.NoContent();
        }

        return Results.Ok(results.ProjectTo(r => r.MapTo<CategoryDto>(r), cancellationToken));
    }
}
