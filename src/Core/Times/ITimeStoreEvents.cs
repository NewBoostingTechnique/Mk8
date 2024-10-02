namespace Mk8.Core.Times;

internal interface ITimeStoreEvents
{
    event EventHandler<CreatedEventArgs>? Created;

    internal sealed class CreatedEventArgs(Time time) : EventArgs
    {
        internal Time Time { get; } = time;
    }
}
