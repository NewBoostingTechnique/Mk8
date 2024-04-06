using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Extensions;

namespace Mk8.Core.Locations.Regions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddRegions(this IServiceCollection services)
    {
        return services
            .AddSingleton<IRegionData, MySqlRegionData>()
            .AddRegionCaching()
            .AddSingleton<IRegionService, RegionService>();
    }

    private static IServiceCollection AddRegionCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        ServiceDescriptor? descriptor = services.GetServiceDescriptor<IRegionData>();

        return services.AddSingleton<IRegionData>
        (
            sp => ActivatorUtilities.CreateInstance<CachingRegionData>
            (
                sp,
                (IRegionData)descriptor.CreateInstance(sp)
            )
        );
    }
}