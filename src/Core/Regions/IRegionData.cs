using System.Collections.Immutable;

namespace Mk8.Core.Regions;

public interface IRegionData
{
    Task CreateAsync(Region region, CancellationToken cancellationToken = default);

    Task<Ulid?> IdentifyAsync(string name, CancellationToken cancellationToken = default);

    Task<IImmutableList<Region>> IndexAsync(Ulid countryId, CancellationToken cancellationToken = default);
}
