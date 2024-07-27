namespace Mk8.Core.Locations.Regions;

internal static class RegionDataExtensions
{
    internal static async Task<Ulid> IdentifyRequiredAsync(this IRegionData regionData, string regionName)
    {
        return await regionData.IdentifyAsync(regionName).ConfigureAwait(false)
            ?? throw new InvalidOperationException($"Region '{regionName}' not found.");
    }
}
