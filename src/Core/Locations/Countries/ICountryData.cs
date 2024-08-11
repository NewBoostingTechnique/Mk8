using System.Collections.Immutable;

namespace Mk8.Core.Locations.Countries;

public interface ICountryData
{
    Task<Ulid?> IdentifyAsync(string name);

    Task InsertAsync(Country country);

    Task<IImmutableList<Country>> ListAsync();
}
