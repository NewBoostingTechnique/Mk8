using System.Collections.Immutable;

namespace Mk8.Core.Countries;

internal sealed class EventingCountryData(
    ICountryData innerData
) : ICountryData, ICountryDataEvents
{
    public Task<Ulid?> IdentifyAsync(string name, CancellationToken cancellationToken = default)
    {
        return innerData.IdentifyAsync(name, cancellationToken);
    }

    #region Insert.

    public event EventHandler<ICountryDataEvents.InsertedEventArgs>? Inserted;

    public async Task CreateAsync(Country country, CancellationToken cancellationToken = default)
    {
        await innerData.CreateAsync(country, cancellationToken).ConfigureAwait(false);
        Inserted?.Invoke(this, new ICountryDataEvents.InsertedEventArgs(country));
    }

    #endregion Insert.

    public Task<IImmutableList<Country>> IndexAsync(CancellationToken cancellationToken = default)
    {
        return innerData.IndexAsync(cancellationToken);
    }
}
