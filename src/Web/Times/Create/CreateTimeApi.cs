using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using Mk8.Core.Times.Create;

namespace Mk8.Web.Times.Create;

internal static class CreateTimeApi
{
    internal static void MapCreateTimeApi(this IEndpointRouteBuilder builder)
    {
        // TODO: Add test(s) for backend.

        // TODO: Require tests on PRs (not on main).

        // TODO: Add client-side validation.

        // TODO: Convert front-end to Typescript + Bun.

        // TODO: Add front-end tests.

        builder.MapPost
        (
            "/api/times/",
            async (
                [FromBody] CreateTimeRequest? request,
                [FromServices] ICommandHandler<CreateTimeCommand, Result> handler,
                CancellationToken cancellationToken = default
            ) =>
            {
                request ??= CreateTimeRequest.Default;
                if (!request.Validate(out var errors))
                    return Results.ValidationProblem(errors);

                Result result = await handler.Handle
                (
                    new CreateTimeCommand
                    {
                        CourseName = request.CourseName,
                        Date = request.Date.Value,
                        PlayerName = request.PlayerName,
                        Span = request.Span.Value
                    },
                    cancellationToken
                );
                return result.ToMinimalApiResult();
            }
        );
    }
}
