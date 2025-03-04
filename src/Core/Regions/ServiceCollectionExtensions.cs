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

        services.AddSingleton(sp => ActivatorUtilities.CreateInstance<EventingRegionStore>
        (
            sp,
            (IRegionStore)descriptor.CreateInstance(sp)
        ));

        services.AddSingleton<IRegionDataEvents>(sp => sp.GetRequiredService<EventingRegionStore>());

        services.AddSingleton<IRegionStore>
        (
            sp =>
            {
                EventingRegionStore eventingRegionData = sp.GetRequiredService<EventingRegionStore>();
                return ActivatorUtilities.CreateInstance<CachingRegionStore>
                (
                    sp,
                    eventingRegionData,
                    eventingRegionData
                );
            }
        );
    }
}
