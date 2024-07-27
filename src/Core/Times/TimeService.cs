using Mk8.Core.Courses;
using Mk8.Core.Players;

namespace Mk8.Core.Times;

internal class TimeService(
    ICourseData courseData,
    IPlayerData playerData,
    ITimeData timeData
) : ITimeService
{
    public async Task<bool> ExistsAsync(string courseName, string playerName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(courseName);
        ArgumentException.ThrowIfNullOrWhiteSpace(playerName);

        Ulid? courseId = await courseData.IdentifyAsync(courseName).ConfigureAwait(false);
        if (!courseId.HasValue)
            return false;

        Ulid? playerId = await playerData.IdentifyAsync(playerName).ConfigureAwait(false);
        if (!playerId.HasValue)
            return false;

        return await timeData.ExistsAsync(courseId.Value, playerId.Value).ConfigureAwait(false);
    }

    public async Task<Time> InsertAsync(Time time)
    {
        ArgumentNullException.ThrowIfNull(time);
        ArgumentException.ThrowIfNullOrEmpty(time.CourseName);
        ArgumentException.ThrowIfNullOrEmpty(time.PlayerName);

        time.Id = Ulid.NewUlid();
        time.CourseId = await courseData.IdentifyRequiredAsync(time.CourseName).ConfigureAwait(false);
        time.PlayerId = await playerData.IdentifyRequiredAsync(time.PlayerName).ConfigureAwait(false);

        await timeData.InsertAsync(time).ConfigureAwait(false);

        return time;
    }
}
