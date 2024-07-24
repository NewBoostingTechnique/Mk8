namespace Mk8.Core.Times;

internal interface ITimeDataEvents
{
    event EventHandler<InsertedEventArgs>? Inserted;

    internal sealed class InsertedEventArgs(Time time) : EventArgs
    {
        internal Time Time { get; } = time;
    }
}
