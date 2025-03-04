using Microsoft.Extensions.DependencyInjection;
using Mk8.Management.Core;

namespace Mk8.Management.MySql.Players;

internal static class ServiceCollectionExtensions
{
    internal static void AddPlayers(this IServiceCollection services)
    {
        services.AddSingleton<IStoreManager, MySqlPlayerStoreManager>();
    }
}
