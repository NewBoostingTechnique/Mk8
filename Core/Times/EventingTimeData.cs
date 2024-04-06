namespace Mk8.Core.Times;

internal class EventingTimeData(ITimeData innerData) : ITimeData, ITimeDataEvents
{
    public Task<bool> ExistsAsync(string courseId, string playerId)
    {
        return innerData.ExistsAsync(courseId, playerId);
    }

    #region Insert.

    public event EventHandler<ITimeDataEvents.InsertedEventArgs>? Inserted;

    public async Task InsertAsync(Time time)
    {
        await innerData.InsertAsync(time).ConfigureAwait(false);
        Inserted?.Invoke(this, new ITimeDataEvents.InsertedEventArgs(time));
    }

    #endregion Insert.

}