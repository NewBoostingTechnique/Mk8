using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.News;

namespace Mk8.MySql.News;

internal static class ServiceCollectionExtensions
{
    internal static void AddNews(this IServiceCollection services)
    {
        services.AddSingleton<INewStore, MySqlNewData>();
    }
}
