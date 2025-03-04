using Microsoft.Extensions.DependencyInjection;
using Mk8.Management.Core;

namespace Mk8.Management.MySql.News;

internal static class ServiceCollectionExtensions
{
    internal static void AddNews(this IServiceCollection services)
    {
        services.AddSingleton<IStoreManager, MySqlNewStoreManager>();
    }
}
