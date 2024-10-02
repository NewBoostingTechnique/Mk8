using Mk8.Core.Courses;
using Mk8.Core.Players;

namespace Mk8.Core.Times;

internal class TimeService(
    ICourseData courseStore,
    IPlayerStore playerStore,
    ITimeStore timeStore
) : ITimeService
{
    public async Task<bool> ExistsAsync(string courseName, string playerName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(courseName);
        ArgumentException.ThrowIfNullOrWhiteSpace(playerName);

        Ulid? courseId = await courseStore.IdentifyAsync(courseName).ConfigureAwait(false);
        if (!courseId.HasValue)
            return false;

        Ulid? playerId = await playerStore.IdentifyAsync(playerName).ConfigureAwait(false);
        if (!playerId.HasValue)
            return false;

        return await timeStore.ExistsAsync(courseId.Value, playerId.Value).ConfigureAwait(false);
    }

    public async Task<Time> InsertAsync(Time time)
    {
        ArgumentNullException.ThrowIfNull(time);
        ArgumentException.ThrowIfNullOrEmpty(time.CourseName);
        ArgumentException.ThrowIfNullOrEmpty(time.PlayerName);

        await timeStore.CreateAsync
        (
            time with
            {
                Id = Ulid.NewUlid(),
                CourseId = await courseStore.IdentifyRequiredAsync(time.CourseName).ConfigureAwait(false),
                PlayerId = await playerStore.IdentifyRequiredAsync(time.PlayerName).ConfigureAwait(false)
            }
        )
        .ConfigureAwait(false);

        return time;
    }
}
