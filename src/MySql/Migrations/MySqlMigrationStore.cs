using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Mk8.Core.Migrations;
using Mk8.Data.Common;
using MySql.Data.MySqlClient;

namespace Mk8.MySql.Migrations;

sealed internal class MySqlMigrationStore(
    IOptions<MySqlSettings> options
) : IMigrationStore
{
    public async Task CreateAsync(Migration migration, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(options.Value.ConnectionString);

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
        using MySqlConnection connection = new(options.Value.ConnectionString);

        using MySqlCommand command = new("migration_detail", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Id", id);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

        if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            throw new KeyNotFoundException();

        return new()
        {
            Id = id,
            Description = reader.GetString("Description"),
            Progress = reader.GetByte("Progress"),
            Error = reader.GetStringNullable("Error"),
            StartTime = reader.GetDateTime("StartTime"),
            EndTime = reader.GetDateTimeNullable("EndTime")
        };
    }

    public async Task<ImmutableList<Migration>> IndexAsync(Migration? after, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(options.Value.ConnectionString);

        using MySqlCommand command = new("migration_index", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("AfterId", after?.Id);
        command.AddParameter("AfterStartTime", after?.StartTime);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

        ImmutableList<Migration>.Builder builder = ImmutableList.CreateBuilder<Migration>();
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            builder.Add(new()
            {
                Id = reader.GetUlid("Id"),
                Description = reader.GetString("Description"),
                Progress = reader.GetByte("Progress"),
                Error = reader.GetStringNullable("Error"),
                StartTime = reader.GetDateTime("StartTime"),
                EndTime = reader.GetDateTimeNullable("EndTime")
            });
        }

        return builder.ToImmutable();
    }

    public async Task UpdateAsync(Migration migration, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(options.Value.ConnectionString);

        using MySqlCommand command = new("migration_update", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Id", migration.Id);
        command.AddParameter("Progress", migration.Progress);
        command.AddParameter("Error", migration.Error);
        command.AddParameter("EndTime", migration.EndTime);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }
}
