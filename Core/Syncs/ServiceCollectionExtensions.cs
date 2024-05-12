using Microsoft.Extensions.DependencyInjection;

namespace Mk8.Core.Syncs;

internal static class ServiceCollectionExtensions
{
    internal static void AddSyncs(this IServiceCollection services)
    {
        services.AddSingleton<ISyncService, SyncService>();
    }
}