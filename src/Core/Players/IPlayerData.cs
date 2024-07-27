using System.Collections.Immutable;

namespace Mk8.Core.Players;

public interface IPlayerData
{
    Task DeleteAsync(Ulid id);

    Task<Player?> DetailAsync(Ulid id);

    Task<bool> ExistsAsync(string name);

    Task<Ulid?> IdentifyAsync(string name);

    Task InsertAsync(Player player);

    Task<IImmutableList<Player>> ListAsync();
}
