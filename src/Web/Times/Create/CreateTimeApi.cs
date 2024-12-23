using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using Mk8.Core.Times.Create;

namespace Mk8.Web.Times.CreateTime;

internal static class CreateTimeApi
{
    internal static void MapCreateTime(this IEndpointRouteBuilder builder)
    {
        // TODO: Perhaps all these will call straight to the command handler.
        // So can we put a generic layer to convert the results into minimal API results.

        builder.MapPost
        (
            "/api/times/",
            async (
                [FromBody] CreateTimeCommand command,
                [FromServices] ICommandHandler<CreateTimeCommand, Result> handler,
                CancellationToken cancellationToken = default
            ) =>
            {
                // TODO: Test valid request data is required.
                Result result = await handler.Handle(command, cancellationToken);
                return result.ToMinimalApiResult();
            }
        );
    }
}
