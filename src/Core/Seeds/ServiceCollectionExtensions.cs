using Microsoft.Extensions.DependencyInjection;

namespace Mk8.Core.Seeds;

internal static class ServiceCollectionExtensions
{
    internal static void AddSeeds(this IServiceCollection services)
    {
        services.AddSingleton<ISeedService, SeedService>();
    }
}
