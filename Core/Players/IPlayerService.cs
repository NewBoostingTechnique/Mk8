using System.Collections.Immutable;

namespace Mk8.Core.Players;

public interface IPlayerService
{
    Task DeleteAsync(string playerName);

    Task<bool> ExistsAsync(string playerName);

    Task<Player?> FindAsync(string playerName);

    Task<Player> InsertAsync(Player player);

    Task<IImmutableList<Player>> ListAsync();
}
