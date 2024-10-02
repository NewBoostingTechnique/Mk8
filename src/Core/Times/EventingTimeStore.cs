namespace Mk8.Core.Times;

internal class EventingTimeStore(ITimeStore innerStore) : ITimeStore, ITimeStoreEvents
{

    #region Create.

    public event EventHandler<ITimeStoreEvents.CreatedEventArgs>? Created;

    public async Task CreateAsync(Time time)
    {
        await innerStore.CreateAsync(time).ConfigureAwait(false);
        Created?.Invoke(this, new ITimeStoreEvents.CreatedEventArgs(time));
    }

    #endregion Create.

    public Task<bool> ExistsAsync(Ulid courseId, Ulid playerId)
    {
        return innerStore.ExistsAsync(courseId, playerId);
    }
}
