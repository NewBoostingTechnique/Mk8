using System.Collections.Immutable;

namespace Mk8.Core.Countries;

internal class CountryService(
    ICountryStore countryStore
) : ICountryService
{
    public Task<IImmutableList<Country>> IndexAsync()
    {
        return countryStore.IndexAsync();
    }
}
