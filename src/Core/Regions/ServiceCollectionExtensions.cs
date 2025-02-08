using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.DependencyInjection;

namespace Mk8.Core.Regions;

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

        ServiceDescriptor? descriptor = services.GetServiceDescriptor<IRegionStore>();

        services.AddSingleton(sp => ActivatorUtilities.CreateInstance<EventingRegionData>
        (
            sp,
            (IRegionStore)descriptor.CreateInstance(sp)
        ));

        services.AddSingleton<IRegionDataEvents>(sp => sp.GetRequiredService<EventingRegionData>());

        services.AddSingleton<IRegionStore>
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
