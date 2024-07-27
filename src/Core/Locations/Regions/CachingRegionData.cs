using System.Collections.Immutable;
using Microsoft.Extensions.Caching.Memory;

namespace Mk8.Core.Locations.Regions;

internal class CachingRegionData(IMemoryCache cache, IRegionData innerData) : IRegionData
{
    public Task<Ulid?> IdentifyAsync(string regionName)
    {
        return cache.GetOrCreateAsync
        (
            $"Region_Identify:{regionName}",
            entry => innerData.IdentifyAsync(regionName)
        );
    }

    public Task<IImmutableList<Region>> ListAsync(Ulid countryId)
    {
        return cache.GetOrCreateAsync
        (
            $"Region_List:{countryId}",
            entry => innerData.ListAsync(countryId)
        )!;
    }
}
