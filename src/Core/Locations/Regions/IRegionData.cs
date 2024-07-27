using System.Collections.Immutable;

namespace Mk8.Core.Locations.Regions;

public interface IRegionData
{
    Task<Ulid?> IdentifyAsync(string regionName);

    Task<IImmutableList<Region>> ListAsync(Ulid countryId);
}
