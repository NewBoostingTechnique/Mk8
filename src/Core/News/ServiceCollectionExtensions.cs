using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.DependencyInjection;

namespace Mk8.Core.News;

internal static class ServiceCollectionExtensions
{
    internal static void AddNews(this IServiceCollection services)
    {
        services.AddNewsCaching();
        services.AddSingleton<INewService, NewService>();
    }

    private static void AddNewsCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        ServiceDescriptor? descriptor = services.GetServiceDescriptor<INewStore>();

        services.AddSingleton(sp => ActivatorUtilities.CreateInstance<EventingNewData>
        (
            sp,
            (INewStore)descriptor.CreateInstance(sp))
        );

        services.AddSingleton<INewStoreEvents>(sp => sp.GetRequiredService<EventingNewData>());

        services.AddSingleton<INewStore>
        (
            sp =>
            {
                EventingNewData eventingNewData = sp.GetRequiredService<EventingNewData>();
                return ActivatorUtilities.CreateInstance<CachingNewStore>
                (
                    sp,
                    eventingNewData,
                    eventingNewData
                );
            }
        );
    }
}
