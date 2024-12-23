using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.DependencyInjection;
using Mk8.Core.Times.Create;

namespace Mk8.Core.Times;

internal static class ServiceCollectionExtensions
{
    internal static void AddTimes(this IServiceCollection services)
    {
        services.AddTimeCaching();
        services.AddSingleton<ICommandHandler<CreateTimeCommand, Result>, CreateTimeHandler>();
    }

    private static IServiceCollection AddTimeCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        ServiceDescriptor? descriptor = services.GetServiceDescriptor<ITimeStore>();

        services.AddSingleton(sp => ActivatorUtilities.CreateInstance<EventingTimeStore>
        (
            sp,
            (ITimeStore)descriptor.CreateInstance(sp)
        ));

        services.AddSingleton<ITimeStoreEvents>(sp => sp.GetRequiredService<EventingTimeStore>());

        return services.AddSingleton<ITimeStore>
        (
            sp =>
            {
                EventingTimeStore eventingTimeData = sp.GetRequiredService<EventingTimeStore>();
                return ActivatorUtilities.CreateInstance<CachingTimeStore>
                (
                    sp,
                    eventingTimeData,
                    eventingTimeData
                );
            }
        );
    }
}
