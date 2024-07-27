namespace Mk8.Core.Locations.Countries;

internal static class CountryDataExtensions
{
    internal static async Task<Ulid> IdentifyRequiredAsync(this ICountryData countryData, string countryName)
    {
        return await countryData.IdentifyAsync(countryName).ConfigureAwait(false)
            ?? throw new InvalidOperationException($"Country '{countryName}' not found.");
    }
}
