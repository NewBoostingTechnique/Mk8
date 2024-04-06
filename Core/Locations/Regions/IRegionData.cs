using System.Collections.Immutable;

namespace Mk8.Core.Locations.Regions;

internal interface IRegionData
{
    Task<string?> IdentifyAsync(string regionName);

    Task<IImmutableList<Region>> ListAsync(string countryId);
}