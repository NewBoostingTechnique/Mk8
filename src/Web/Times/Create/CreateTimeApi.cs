using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mk8.Core.Times.Create;

namespace Mk8.Web.Times.Create;

internal static class CreateTimeApi
{
    internal static void MapCreateTimeApi(this IEndpointRouteBuilder builder)
    {
        // TODO: Add test(s) for backend.
        // 6. Conflict - time already exists.

        // TODO: Create issue for:
        // Entities should not be public.
        // Consider adding a test API for creating entities.

        // TODO: Require tests on PRs (not on main).

        // TODO: Add client-side validation.

        // TODO: Convert front-end to Typescript + Bun.

        // TODO: Add front-end tests.

        builder.MapPost
        (
            "/api/times/",
            async (
                [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] CreateTimeRequest request,
                [FromServices] ICommandHandler<CreateTimeCommand, Result> handler,
                CancellationToken cancellationToken = default
            ) =>
            {
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
