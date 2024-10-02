using Mk8.Core.News;
using Mk8.Core.Players;
using System.Collections.Immutable;

namespace Mk8.Core.Migrations;

internal class MigrationService(
    IMigrationStore migrationStore,
    INewService newService,
    IPlayerService playerService
) : IMigrationService
{
    public async Task<Migration> CreateAsync(CancellationToken cancellationToken = default)
    {
        Migration migration = await migrationStore.StartAsync("Full System Data Migration", cancellationToken).ConfigureAwait(false);

        _ = Task.Run
        (
            async () =>
            {
                string? error = null;
                try
                {
                    Migration[] migrations = await Task.WhenAll
                    (
                        newService.MigrateAsync(cancellationToken),
                        playerService.MigrateAsync(cancellationToken)
                    )
                    .ConfigureAwait(false);

                    do
                    {
                        await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
                        byte progress = (byte)Math.Floor(migrations.Average(m => m.Progress));
                        await migrationStore.UpdateAsync(migration, progress, cancellationToken).ConfigureAwait(false);
                    }
                    while (true);
                }
                catch (Exception ex)
                {
                    error = ex.ToString();
                }

                await migrationStore.UpdateAsync
                (
                    migration with
                    {
                        EndTime = DateTime.UtcNow,
                        Progress = 100,
                        Error = error
                    },
                    CancellationToken.None
                )
                .ConfigureAwait(false);
            },
            cancellationToken
        );

        return migration;
    }

    public Task<Migration?> FindAsync(Ulid id, CancellationToken cancellationToken = default)
    {
        return migrationStore.FindAsync(id, cancellationToken);
    }

    public Task<IImmutableList<Migration>> IndexAsync(CancellationToken cancellationToken = default)
    {
        return migrationStore.IndexAsync(cancellationToken);
    }
}
