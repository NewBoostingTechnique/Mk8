using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Extensions;

namespace Mk8.Core.News;

internal static class ServiceCollectionExtensions
{
    internal static void AddNews(this IServiceCollection services)
    {
        services.AddSingleton<INewSource, Mk8NewsScraper>();
        services.AddSingleton<INewSync, NewSync>();
        services.AddNewsCaching();
        services.AddSingleton<INewService, NewService>();
    }

    private static void AddNewsCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        ServiceDescriptor? descriptor = services.GetServiceDescriptor<INewData>();

        services.AddSingleton(sp => ActivatorUtilities.CreateInstance<EventingNewData>
        (
            sp,
            (INewData)descriptor.CreateInstance(sp))
        );

        services.AddSingleton<INewDataEvents>(sp => sp.GetRequiredService<EventingNewData>());

        services.AddSingleton<INewData>
        (
            sp =>
            {
                EventingNewData eventingNewData = sp.GetRequiredService<EventingNewData>();
                return ActivatorUtilities.CreateInstance<CachingNewData>
                (
                    sp,
                    eventingNewData,
                    eventingNewData
                );
            }
        );
    }
}