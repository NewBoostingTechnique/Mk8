using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Mk8.Core;
using Mk8.Core.Migrations;
using Mk8.Data.Common;
using MySql.Data.MySqlClient;

namespace Mk8.MySql.Migrations;

internal class MySqlMigrationStore(IOptions<Mk8Settings> mk8Options) : IMigrationStore
{
    public async Task CreateAsync(Migration migration, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("migration_create", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Id", migration.Id);
        command.AddParameter("Description", migration.Description);
        command.AddParameter("StartTime", migration.StartTime);
        command.AddParameter("Progress", migration.Progress);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<Migration?> DetailAsync(Ulid id, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("migration_detail", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter(nameof(Migration.Id), id);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

        if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            throw new KeyNotFoundException();

        return new()
        {
            Id = id,
            Description = reader.GetString(nameof(Migration.Description)),
            Progress = reader.GetByte(nameof(Migration.Progress)),
            Error = reader.GetStringNullable(nameof(Migration.Error)),
            StartTime = reader.GetDateTime(nameof(Migration.StartTime)),
            EndTime = reader.GetDateTimeNullable(nameof(Migration.EndTime))
        };
    }

    public async Task<IImmutableList<Migration>> IndexAsync(CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("migration_index", connection);
        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

        ImmutableList<Migration>.Builder builder = ImmutableList.CreateBuilder<Migration>();
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            builder.Add(new()
            {
                Id = reader.GetUlid(nameof(Migration.Id)),
                Description = reader.GetString(nameof(Migration.Description)),
                Progress = reader.GetByte(nameof(Migration.Progress)),
                Error = reader.GetStringNullable(nameof(Migration.Error)),
                StartTime = reader.GetDateTime(nameof(Migration.StartTime)),
                EndTime = reader.GetDateTimeNullable(nameof(Migration.EndTime))
            });
        }

        return builder.ToImmutable();
    }

    public async Task UpdateAsync(Migration migration, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("migration_update", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter(nameof(Migration.Id), migration.Id);
        command.AddParameter(nameof(Migration.Progress), migration.Progress);
        command.AddParameter(nameof(Migration.Error), migration.Error);
        command.AddParameter(nameof(Migration.EndTime), migration.EndTime);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }
}
