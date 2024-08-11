namespace Mk8.Core.Locations.Countries;

internal static class CountryDataExtensions
{
    internal static async Task<Ulid> IdentifyRequiredAsync(this ICountryData countryData, string name)
    {
        return await countryData.IdentifyAsync(name).ConfigureAwait(false)
            ?? throw new InvalidOperationException($"Country '{name}' not found.");
    }
}
