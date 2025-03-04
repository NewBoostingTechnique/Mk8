namespace Mk8.Core.Regions;

internal static class RegionStoreExtensions
{
    internal static async Task<Ulid> IdentifyRequiredAsync(this IRegionStore regionStore, string regionName)
    {
        return await regionStore.IdentifyAsync(regionName).ConfigureAwait(false)
            ?? throw new InvalidOperationException($"Region '{regionName}' not found.");
    }
}
