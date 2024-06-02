using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Locations.Regions;

namespace Mk8.MySql.Locations.Regions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddRegions(this IServiceCollection services)
    {
        return services.AddSingleton<IRegionData, MySqlRegionData>();
    }
}