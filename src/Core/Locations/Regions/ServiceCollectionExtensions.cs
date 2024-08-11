using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.DependencyInjection;

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

        services.AddSingleton(sp => ActivatorUtilities.CreateInstance<EventingRegionData>
        (
            sp,
            (IRegionData)descriptor.CreateInstance(sp)
        ));

        services.AddSingleton<IRegionDataEvents>(sp => sp.GetRequiredService<EventingRegionData>());

        services.AddSingleton<IRegionData>
        (
            sp =>
            {
                EventingRegionData eventingRegionData = sp.GetRequiredService<EventingRegionData>();
                return ActivatorUtilities.CreateInstance<CachingRegionData>
                (
                    sp,
                    eventingRegionData,
                    eventingRegionData
                );
            }
        );
    }
}
