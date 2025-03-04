using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Countries;
using Mk8.Core.Regions;

namespace Mk8.MySql.Test.Regions;

public class IndexTest : MySqlTest
{
    [Test]
    public async Task ShouldReturnRegionsAsync()
    {
        // Arrange.
        IServiceProvider serviceProvider = await ServiceProviderTask;
        ICountryStore countryStore = serviceProvider.GetRequiredService<ICountryStore>();
        IRegionStore regionStore = serviceProvider.GetRequiredService<IRegionStore>();

        Country country = new()
        {
            Id = Ulid.NewUlid(),
            Name = "United Kingdom"
        };
        await countryStore.CreateAsync(country);

        Region[] expectedRegions = [
            new()
            {
                Id = Ulid.NewUlid(),
                Name = "Guildford",
                CountryId = country.Id.Value
            },
            new()
            {
                Id = Ulid.NewUlid(),
                Name = "Watford",
                CountryId = country.Id.Value
            }
        ];
        foreach (Region region in expectedRegions)
        {
            await regionStore.CreateAsync(region);
        }

        // Act.
        IImmutableList<Region> actualRegions = await regionStore.IndexAsync(country.Id.Value);

        // Assert.
        Assert.That(actualRegions.Count, Is.EqualTo(expectedRegions.Length));
        Assert.Multiple(() =>
        {
            foreach (Region expectedRegion in expectedRegions)
            {
                Region? actualRegion = actualRegions.FirstOrDefault(a => a.Name == expectedRegion.Name);
                Assert.That(actualRegion?.CountryId, Is.EqualTo(expectedRegion.CountryId));
            }
        });
    }
}
