using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.DependencyInjection;

namespace Mk8.Core.Players;

internal static class ServiceCollectionExtensions
{
    internal static void AddPlayers(this IServiceCollection services)
    {
        services.AddPlayerCaching();
        services.AddHttpClient();
        services.AddSingleton<IPlayerService, PlayerService>();
    }

    private static void AddPlayerCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        ServiceDescriptor? descriptor = services.GetServiceDescriptor<IPlayerStore>();

        services.AddSingleton(sp => ActivatorUtilities.CreateInstance<EventingPlayerStore>
        (
            sp,
            (IPlayerStore)descriptor.CreateInstance(sp)
        ));

        services.AddSingleton<IPlayerStoreEvents>(sp => sp.GetRequiredService<EventingPlayerStore>());

        services.AddSingleton<IPlayerStore>
        (
            sp =>
            {
                EventingPlayerStore eventingPlayerData = sp.GetRequiredService<EventingPlayerStore>();
                return ActivatorUtilities.CreateInstance<CachingPlayerStore>
                (
                    sp,
                    eventingPlayerData,
                    eventingPlayerData
                );
            }
        );
    }
}
