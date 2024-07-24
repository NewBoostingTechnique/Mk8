using System.Collections.Immutable;

namespace Mk8.Core.Locations.Countries;

internal class CountryService(ICountryData countryData) : ICountryService
{
    public Task<IImmutableList<Country>> ListAsync()
    {
        return countryData.ListAsync();
    }
}
