using System.Collections.Immutable;
using Mk8.Core.Migrations;

namespace Mk8.Core.Players;

public interface IPlayerService
{
    Task<Player> CreateAsync(Player player, CancellationToken cancellationToken = default);

    Task DeleteAsync(string playerName, CancellationToken cancellationToken = default);

    Task<Player?> DetailAsync(string playerName, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(string playerName, CancellationToken cancellationToken = default);

    Task<ImmutableList<Player>> IndexAsync(CancellationToken cancellationToken = default);

    Task<Migration> MigrateAsync(CancellationToken cancellationToken = default);
}
