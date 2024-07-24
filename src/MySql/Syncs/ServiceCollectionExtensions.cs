using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Syncs;

namespace Mk8.MySql.Syncs;

internal static class ServiceCollectionExtensions
{
    internal static void AddSyncs(this IServiceCollection services)
    {
        services.AddSingleton<ISyncData, MySqlSyncData>();
    }
}
