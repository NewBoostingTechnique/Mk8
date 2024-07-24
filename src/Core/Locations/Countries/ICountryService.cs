using System.Collections.Immutable;

namespace Mk8.Core.Locations.Countries;

public interface ICountryService
{
    Task<IImmutableList<Country>> ListAsync();
}
