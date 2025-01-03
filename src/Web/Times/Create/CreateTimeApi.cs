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
        // TODO: Linux only.

        // TODO: Convert front-end to Typescript + Bun.

        // TODO: Add client-side validation.

        // TODO: Add front-end tests.

        // TODO: Squash history?

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
