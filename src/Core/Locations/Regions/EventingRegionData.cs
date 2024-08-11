using System.Collections.Immutable;
using static Mk8.Core.Locations.Regions.IRegionDataEvents;

namespace Mk8.Core.Locations.Regions;

internal class EventingRegionData(
    IRegionData innerData
) : IRegionData, IRegionDataEvents
{
    public Task<Ulid?> IdentifyAsync(string name)
    {
        return innerData.IdentifyAsync(name);
    }

    #region Insert.

    public event EventHandler<InsertedEventArgs>? Inserted;

    public async Task InsertAsync(Region region)
    {
        await innerData.InsertAsync(region).ConfigureAwait(false);
        Inserted?.Invoke(this, new InsertedEventArgs(region));
    }

    #endregion Insert.

    public Task<IImmutableList<Region>> ListAsync(Ulid countryId)
    {
        return innerData.ListAsync(countryId);
    }
}
