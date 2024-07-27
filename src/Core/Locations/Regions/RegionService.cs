using System.Collections.Immutable;
using Mk8.Core.Locations.Countries;

namespace Mk8.Core.Locations.Regions;

internal class RegionService(
    ICountryData countryData,
    IRegionData regionData
) : IRegionService
{
    public async Task<IImmutableList<Region>> ListAsync(string countryName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(countryName);

        Ulid countryId = await countryData.IdentifyRequiredAsync(countryName).ConfigureAwait(false);

        return await regionData.ListAsync(countryId).ConfigureAwait(false);
    }
}
