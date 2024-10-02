using System.Collections.Immutable;

namespace Mk8.Core.Regions;

public interface IRegionService
{
    Task<IImmutableList<Region>> IndexAsync(string countryName);

    Task SeedAsync();
}
