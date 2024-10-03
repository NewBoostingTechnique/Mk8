using System.Collections.Immutable;

namespace Mk8.Core.Migrations;

public interface IMigrationService
{
    Task<Migration> CreateAsync(CancellationToken cancellationToken = default);

    Task<Migration?> DetailAsync(Ulid id, CancellationToken cancellationToken = default);

    // TODO: Pagination.
    Task<IImmutableList<Migration>> IndexAsync(CancellationToken cancellationToken = default);
}
