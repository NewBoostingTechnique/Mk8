using System.Collections.Immutable;
using Microsoft.Extensions.Caching.Memory;

namespace Mk8.Core.Locations.Countries;

internal class CachingCountryData(IMemoryCache cache, ICountryData innerData) : ICountryData
{
    public Task<Ulid?> IdentifyAsync(string countryName)
    {
        return cache.GetOrCreateAsync
        (
            $"Country_Identify:{countryName}",
            entry => innerData.IdentifyAsync(countryName)
        );
    }

    public Task<IImmutableList<Country>> ListAsync()
    {
        return cache.GetOrCreateAsync
        (
            "Country_List",
            entry => innerData.ListAsync()
        )!;
    }
}
