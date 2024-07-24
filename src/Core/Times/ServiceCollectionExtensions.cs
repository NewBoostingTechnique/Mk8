using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Extensions;

namespace Mk8.Core.Times;

internal static class ServiceCollectionExtensions
{
    internal static void AddTimes(this IServiceCollection services)
    {
        services.AddTimeCaching();
        services.AddSingleton<ITimeService, TimeService>();
    }

    private static IServiceCollection AddTimeCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        ServiceDescriptor? descriptor = services.GetServiceDescriptor<ITimeData>();

        services.AddSingleton(sp => ActivatorUtilities.CreateInstance<EventingTimeData>
        (
            sp,
            (ITimeData)descriptor.CreateInstance(sp)
        ));

        services.AddSingleton<ITimeDataEvents>(sp => sp.GetRequiredService<EventingTimeData>());

        return services.AddSingleton<ITimeData>
        (
            sp =>
            {
                EventingTimeData eventingTimeData = sp.GetRequiredService<EventingTimeData>();
                return ActivatorUtilities.CreateInstance<CachingTimeData>
                (
                    sp,
                    eventingTimeData,
                    eventingTimeData
                );
            }
        );
    }
}