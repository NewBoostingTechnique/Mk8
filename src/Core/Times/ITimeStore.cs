namespace Mk8.Core.Times;

public interface ITimeStore
{
    Task CreateAsync(Time time);

    Task<bool> ExistsAsync(Ulid courseId, Ulid playerId);
}
