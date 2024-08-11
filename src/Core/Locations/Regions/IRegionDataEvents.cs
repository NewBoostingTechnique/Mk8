namespace Mk8.Core.Locations.Regions;

internal interface IRegionDataEvents
{

    #region Inserted.

    event EventHandler<InsertedEventArgs>? Inserted;

    internal sealed class InsertedEventArgs(Region region) : EventArgs
    {
        internal Region Region { get; } = region;
    }

    #endregion Inserted.

}
