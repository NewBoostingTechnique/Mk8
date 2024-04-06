using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Extensions;
using Mk8.Core.Players.MySql;

namespace Mk8.Core.Players;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddPlayers(this IServiceCollection services)
    {
        return services
            .AddSingleton<IPlayerData, MySqlPlayerData>()
            .AddPlayerCaching()
            .AddSingleton<IPlayerService, PlayerService>();
    }

    private static IServiceCollection AddPlayerCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        ServiceDescriptor? descriptor = services.GetServiceDescriptor<IPlayerData>();

        services.AddSingleton(sp => ActivatorUtilities.CreateInstance<EventingPlayerData>
        (
            sp,
            (IPlayerData)descriptor.CreateInstance(sp)
        ));

        services.AddSingleton<IPlayerDataEvents>(sp => sp.GetRequiredService<EventingPlayerData>());

        return services.AddSingleton<IPlayerData>
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