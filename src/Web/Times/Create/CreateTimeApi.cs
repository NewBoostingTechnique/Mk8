using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using Mk8.Core.Times.Create;

namespace Mk8.Web.Times.Create;

internal static class CreateTimeApi
{
    internal static void MapCreateTime(this IEndpointRouteBuilder builder)
    {
        // TODO: Api should have dedicated request and response records.

        // TODO: Test valid request data is required.

        // TODO: Add server-side validation with FluentValidation (See Ardalis).

        // TODO: Perhaps all these will call straight to the command handler.
        // So can we put a generic layer to convert the results into minimal API results.

        // TODO: Add test(s) for backend.

        // TODO: Add client-side validation.

        // TODO: Convert front-end to Typescript + Bun.

        // TODO: Add front-end tests.

        builder.MapPost
        (
            "/api/times/",
            async (
                [FromBody] CreateTimeRequest request,
                [FromServices] ICommandHandler<CreateTimeCommand, Result> handler,
                CancellationToken cancellationToken = default
            ) =>
            {
                Result result = await handler.Handle
                (
                    new CreateTimeCommand
                    {
                        CourseName = request.CourseName,
                        Date = request.Date,
                        PlayerName = request.PlayerName,
                        Span = request.Span
                    },
                    cancellationToken
                );
                return result.ToMinimalApiResult();
            }
        );
    }
}
