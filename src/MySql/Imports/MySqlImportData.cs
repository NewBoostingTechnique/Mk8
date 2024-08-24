using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Mk8.Core;
using Mk8.Core.Imports;
using Mk8.Data.Common;
using MySql.Data.MySqlClient;

namespace Mk8.MySql.Imports;

internal class MySqlImportData(IOptions<Mk8Settings> mk8Options) : IImportData
{
    public async Task<Import?> DetailAsync(Ulid id)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("DetailImport", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Id", id);

        await connection.OpenAsync().ConfigureAwait(false);
        using DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

        if (!await reader.ReadAsync().ConfigureAwait(false))
            return default;

        return new()
        {
            Id = id,
            EndTime = reader.GetDateTimeNullable(0),
            Error = reader.GetStringNullable(1),
            StartTime = reader.GetDateTime(2)
        };
    }

    public async Task InsertAsync(Import import)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("InsertImport", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Id", import.Id);
        command.AddParameter("StartTime", import.StartTime);

        await connection.OpenAsync().ConfigureAwait(false);
        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }

    public async Task UpdateAsync(Import import)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("UpdateImport", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("EndTime", import.EndTime);
        command.AddParameter("Error", import.Error);
        command.AddParameter("Id", import.Id);

        await connection.OpenAsync().ConfigureAwait(false);
        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }
}
