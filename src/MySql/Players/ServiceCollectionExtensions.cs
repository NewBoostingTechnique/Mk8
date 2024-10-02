using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Players;

namespace Mk8.MySql.Players;

internal static class ServiceCollectionExtensions
{
    internal static void AddPlayers(this IServiceCollection services)
    {
        services.AddSingleton<IPlayerStore, MySqlPlayerStore>();
    }
}
