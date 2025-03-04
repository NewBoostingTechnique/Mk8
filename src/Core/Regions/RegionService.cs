using System.Collections.Immutable;
using Mk8.Core.Countries;

namespace Mk8.Core.Regions;

internal class RegionService(
    ICountryStore countryStore,
    IRegionStore regionData
) : IRegionService
{
    public async Task<IImmutableList<Region>> IndexAsync(string countryName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(countryName);

        Ulid countryId = await countryStore.IdentifyRequiredAsync(countryName).ConfigureAwait(false);

        return await regionData.IndexAsync(countryId).ConfigureAwait(false);
    }
}
