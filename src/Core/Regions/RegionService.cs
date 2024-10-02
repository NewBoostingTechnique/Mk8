using System.Collections.Immutable;
using Microsoft.Extensions.Logging;
using Mk8.Core.Countries;

namespace Mk8.Core.Regions;

internal class RegionService(
    ICountryData countryData,
    ILogger<RegionService> logger,
    IRegionData regionData
) : IRegionService
{
    public async Task<IImmutableList<Region>> IndexAsync(string countryName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(countryName);

        Ulid countryId = await countryData.IdentifyRequiredAsync(countryName).ConfigureAwait(false);

        return await regionData.IndexAsync(countryId).ConfigureAwait(false);
    }

    public async Task SeedAsync()
    {
        logger.LogInformation("Seeding Regions...");

        await regionData.CreateAsync
        (
            new Region
            {
                Id = Ulid.NewUlid(),
                Name = "Guildford",
                CountryId = await countryData.IdentifyRequiredAsync("United Kingdom").ConfigureAwait(false)
            }
        )
        .ConfigureAwait(false);
    }
}
