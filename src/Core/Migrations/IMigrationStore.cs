using System.Collections.Immutable;

namespace Mk8.Core.Migrations;

public interface IMigrationStore
{
    Task CreateAsync(Migration migration, CancellationToken cancellationToken = default);

    Task<Migration?> DetailAsync(Ulid id, CancellationToken cancellationToken = default);

    Task<IImmutableList<Migration>> IndexAsync(CancellationToken cancellationToken = default);

    Task UpdateAsync(Migration migration, CancellationToken cancellationToken = default);
}
