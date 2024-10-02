using System.Collections.Immutable;

namespace Mk8.Core.Players;

public interface IPlayerStore
{
    Task CreateAsync(Player player, CancellationToken cancellationToken = default);

    Task DeleteAsync(CancellationToken cancellationToken = default);

    Task DeleteAsync(Ulid id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default);

    Task<Player?> FindAsync(Ulid id, CancellationToken cancellationToken = default);

    Task<Ulid?> IdentifyAsync(string name, CancellationToken cancellationToken = default);

    Task<IImmutableList<Player>> IndexAsync(CancellationToken cancellationToken = default);
}
