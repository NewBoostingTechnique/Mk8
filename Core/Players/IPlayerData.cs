using System.Collections.Immutable;

namespace Mk8.Core.Players;

internal interface IPlayerData
{
    Task DeleteAsync(string playerId);

    Task<Player?> DetailAsync(string playerId);

    Task<bool> ExistsAsync(string playerName);

    Task<string?> IdentifyAsync(string playerName);

    Task InsertAsync(Player player);

    Task<IImmutableList<Player>> ListAsync();
}