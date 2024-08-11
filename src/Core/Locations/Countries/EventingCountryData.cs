using System.Collections.Immutable;

namespace Mk8.Core.Locations.Countries;

internal sealed class EventingCountryData(
    ICountryData innerData
) : ICountryData, ICountryDataEvents
{
    public Task<Ulid?> IdentifyAsync(string name)
    {
        return innerData.IdentifyAsync(name);
    }

    #region Insert.

    public event EventHandler<ICountryDataEvents.InsertedEventArgs>? Inserted;

    public async Task InsertAsync(Country country)
    {
        await innerData.InsertAsync(country).ConfigureAwait(false);
        Inserted?.Invoke(this, new ICountryDataEvents.InsertedEventArgs(country));
    }

    #endregion Insert.

    public Task<IImmutableList<Country>> ListAsync()
    {
        return innerData.ListAsync();
    }
}
