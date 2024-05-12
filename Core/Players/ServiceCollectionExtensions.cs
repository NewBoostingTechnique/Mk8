using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Extensions;
using Mk8.Core.Players.MySql;

namespace Mk8.Core.Players;

internal static class ServiceCollectionExtensions
{
    internal static void AddPlayers(this IServiceCollection services)
    {
        services.AddSingleton<IPlayerData, MySqlPlayerData>();
        services.AddPlayerCaching();
        services.AddSingleton<IPlayerService, PlayerService>();
    }

    private static void AddPlayerCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        ServiceDescriptor? descriptor = services.GetServiceDescriptor<IPlayerData>();

        services.AddSingleton(sp => ActivatorUtilities.CreateInstance<EventingPlayerData>
        (
            sp,
            (IPlayerData)descriptor.CreateInstance(sp)
        ));

        services.AddSingleton<IPlayerDataEvents>(sp => sp.GetRequiredService<EventingPlayerData>());

        services.AddSingleton<IPlayerData>
        (
            sp =>
            {
                EventingPlayerData eventingPlayerData = sp.GetRequiredService<EventingPlayerData>();
                return ActivatorUtilities.CreateInstance<CachingPlayerData>
                (
                    sp,
                    eventingPlayerData,
                    eventingPlayerData
                );
            }
        );
    }
}