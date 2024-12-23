using Ardalis.Result;
using Ardalis.SharedKernel;
using Mk8.Core.Courses;
using Mk8.Core.Players;

namespace Mk8.Core.Times.Create;

internal class CreateTimeHandler(
    ICourseStore courseStore,
    IPlayerStore playerStore,
    ITimeStore timeStore
) : ICommandHandler<CreateTimeCommand, Result>
{
    public async Task<Result> Handle(CreateTimeCommand request, CancellationToken cancellationToken)
    {
        Ulid? courseId = await courseStore.IdentifyAsync(request.CourseName, cancellationToken).ConfigureAwait(false);
        if (!courseId.HasValue)
            return Result.NotFound(nameof(request.CourseName));

        Ulid? playerId = await playerStore.IdentifyAsync(request.PlayerName, cancellationToken).ConfigureAwait(false);
        if (!playerId.HasValue)
            return Result.NotFound(nameof(request.PlayerName));

        bool timeExists = await timeStore.ExistsAsync(courseId.Value, playerId.Value, cancellationToken).ConfigureAwait(false);
        if (timeExists)
            return Result.Conflict(nameof(request));

        await timeStore.CreateAsync
        (
            new Time
            {
                Id = Ulid.NewUlid(),
                Date = request.Date,
                Span = request.Span,
                CourseId = courseId,
                PlayerId = playerId
            },
            cancellationToken
        )
        .ConfigureAwait(false);

        return Result.Success();
    }
}
