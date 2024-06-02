using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Extensions;

namespace Mk8.Core.Locations.Regions;

internal static class ServiceCollectionExtensions
{
    internal static void AddRegions(this IServiceCollection services)
    {
        services.AddRegionCaching();
        services.AddSingleton<IRegionService, RegionService>();
    }

    private static void AddRegionCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        ServiceDescriptor? descriptor = services.GetServiceDescriptor<IRegionData>();

        services.AddSingleton<IRegionData>
        (
            sp => ActivatorUtilities.CreateInstance<CachingRegionData>
            (
                sp,
                (IRegionData)descriptor.CreateInstance(sp)
            )
        );
    }
}