namespace Mk8.Core.Times;

public interface ITimeData
{
    Task<bool> ExistsAsync(Ulid courseId, Ulid playerId);

    Task InsertAsync(Time time);
}
