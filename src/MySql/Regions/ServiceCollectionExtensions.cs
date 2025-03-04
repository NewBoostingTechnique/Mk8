using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Regions;

namespace Mk8.MySql.Regions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddRegions(this IServiceCollection services)
    {
        return services.AddSingleton<IRegionStore, MySqlRegionStore>();
    }
}
