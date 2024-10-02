using System.Collections.Immutable;

namespace Mk8.Core.Countries;

public interface ICountryData
{
    Task CreateAsync(Country country, CancellationToken cancellationToken = default);

    Task<Ulid?> IdentifyAsync(string name, CancellationToken cancellationToken = default);

    Task<IImmutableList<Country>> IndexAsync(CancellationToken cancellationToken = default);
}
