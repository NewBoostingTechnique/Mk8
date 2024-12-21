namespace Mk8.Core.Countries;

internal static class CountryStoreExtensions
{
    internal static async Task<Ulid> IdentifyRequiredAsync(this ICountryStore countryStore, string name)
    {
        return await countryStore.IdentifyAsync(name).ConfigureAwait(false)
            ?? throw new InvalidOperationException($"Country '{name}' not found.");
    }
}
