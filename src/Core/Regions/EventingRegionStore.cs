using System.Collections.Immutable;
using static Mk8.Core.Regions.IRegionDataEvents;

namespace Mk8.Core.Regions;

internal class EventingRegionStore(
    IRegionStore innerStore
) : IRegionStore, IRegionDataEvents
{
    public Task<Ulid?> IdentifyAsync(string name, CancellationToken cancellationToken = default)
    {
        return innerStore.IdentifyAsync(name, cancellationToken);
    }

    #region Insert.

    public event EventHandler<InsertedEventArgs>? Inserted;

    public async Task CreateAsync(Region region, CancellationToken cancellationToken = default)
    {
        await innerStore.CreateAsync(region, cancellationToken).ConfigureAwait(false);
        Inserted?.Invoke(this, new InsertedEventArgs(region));
    }

    #endregion Insert.

    public Task<IImmutableList<Region>> IndexAsync(Ulid countryId, CancellationToken cancellationToken = default)
    {
        return innerStore.IndexAsync(countryId, cancellationToken);
    }
}
