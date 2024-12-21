using System.Collections.Immutable;

namespace Mk8.Core.Countries;

internal sealed class EventingCountryStore(
    ICountryStore innerStore
) : ICountryStore, ICountryStoreEvents
{
    public Task<Ulid?> IdentifyAsync(string name, CancellationToken cancellationToken = default)
    {
        return innerStore.IdentifyAsync(name, cancellationToken);
    }

    #region Insert.

    public event EventHandler<ICountryStoreEvents.InsertedEventArgs>? Inserted;

    public async Task CreateAsync(Country country, CancellationToken cancellationToken = default)
    {
        await innerStore.CreateAsync(country, cancellationToken).ConfigureAwait(false);
        Inserted?.Invoke(this, new ICountryStoreEvents.InsertedEventArgs(country));
    }

    #endregion Insert.

    public Task<IImmutableList<Country>> IndexAsync(CancellationToken cancellationToken = default)
    {
        return innerStore.IndexAsync(cancellationToken);
    }
}
