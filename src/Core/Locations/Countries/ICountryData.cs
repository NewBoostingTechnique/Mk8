using System.Collections.Immutable;

namespace Mk8.Core.Locations.Countries;

public interface ICountryData
{
    Task<string?> IdentifyAsync(string countryName);

    Task<IImmutableList<Country>> ListAsync();
}