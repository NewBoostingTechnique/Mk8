using System.Collections.Immutable;

namespace Mk8.Core.Locations.Regions;

public interface IRegionService
{
    Task<IImmutableList<Region>> ListAsync(string countryName);

    Task SeedAsync();
}
