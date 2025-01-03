namespace Mk8.Core.Times;

public interface ITimeStore
{
    Task CreateAsync(Time time, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Ulid courseId, Ulid playerId, CancellationToken cancellationToken = default);
}
