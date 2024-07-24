using System.Collections.Immutable;

namespace Mk8.Core.Players;

public interface IPlayerData
{
    Task DeleteAsync(string id);

    Task<Player?> DetailAsync(string id);

    Task<bool> ExistsAsync(string name);

    Task<string?> IdentifyAsync(string name);

    Task InsertAsync(Player player);

    Task<IImmutableList<Player>> ListAsync();
}