using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Mk8.Core;
using Mk8.Core.Syncs;
using Mk8.Data.Common;
using MySql.Data.MySqlClient;

namespace Mk8.MySql.Syncs;

internal class MySqlSyncData(IOptions<Mk8Settings> mk8Options) : ISyncData
{
    public async Task<Sync?> DetailAsync(Ulid id)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("SyncDetail", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Id", id);

        await connection.OpenAsync().ConfigureAwait(false);
        using DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

        if (!await reader.ReadAsync().ConfigureAwait(false))
            return default;

        return new()
        {
            EndTime = reader.GetDateTimeNullable(0),
            Id = id,
            StartTime = reader.GetDateTime(1)
        };
    }

    public async Task InsertAsync(Sync sync)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("SyncInsert", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Id", sync.Id);
        command.AddParameter("StartTime", sync.StartTime);

        await connection.OpenAsync().ConfigureAwait(false);
        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }

    public async Task UpdateAsync(Sync sync)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("SyncUpdate", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("EndTime", sync.EndTime);
        command.AddParameter("Id", sync.Id);

        await connection.OpenAsync().ConfigureAwait(false);
        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }
}
