using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.News.MySql;
using Mk8.Core.Extensions;

namespace Mk8.Core.News;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddNews(this IServiceCollection services)
    {
        return services
            .AddSingleton<INewsData, MySqlNewsData>()
            .AddNewsCaching()
            .AddSingleton<INewsService, NewsService>();
    }

    private static IServiceCollection AddNewsCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        ServiceDescriptor? descriptor = services.GetServiceDescriptor<INewsData>();

        return services.AddSingleton<INewsData>
        (
            sp => ActivatorUtilities.CreateInstance<CachingNewsData>
            (
                sp,
                (INewsData)descriptor.CreateInstance(sp)
            )
        );
    }
}