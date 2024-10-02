using System.Collections.Immutable;

namespace Mk8.Core.Countries;

public interface ICountryService
{
    Task<IImmutableList<Country>> IndexAsync();

    Task SeedAsync();
}
