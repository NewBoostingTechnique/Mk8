namespace Mk8.Core.Migrations;

internal static class MigrationDataExtensions
{
    internal static async Task<Migration> StartAsync(this IMigrationStore migrationData, string description, CancellationToken cancellationToken = default)
    {
        Migration migration = new()
        {
            Id = Ulid.NewUlid(),
            Description = description,
            StartTime = DateTime.UtcNow
        };

        await migrationData.CreateAsync(migration, cancellationToken).ConfigureAwait(false);

        return migration;
    }

    internal static Task UpdateAsync(this IMigrationStore migrationData, Migration migration, byte progress, CancellationToken cancellationToken = default)
    {
        if (progress == migration.Progress)
            return Task.CompletedTask;

        return migrationData.UpdateAsync
        (
            migration with { Progress = progress },
            cancellationToken
        );
    }
}
