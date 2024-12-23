using System.Collections.Immutable;

namespace Mk8.Core.Players;

public interface IPlayerStore
{
    Task CreateAsync(Player player, CancellationToken cancellationToken = default);

    Task DeleteAsync(CancellationToken cancellationToken = default);

    Task DeleteAsync(Ulid id, CancellationToken cancellationToken = default);

    Task<Player?> DetailAsync(Ulid id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default);

    Task<Ulid?> IdentifyAsync(string name, CancellationToken cancellationToken = default);

    Task<ImmutableList<Player>> IndexAsync(CancellationToken cancellationToken = default);
}
