
using System.Data;
using System.Globalization;
using Microsoft.Extensions.Options;
using Mk8.Core.Extensions;
using MySql.Data.MySqlClient;

namespace Mk8.Core.Times.MySql;

internal class MySqlTimeData(IOptions<Mk8Settings> mk8Options) : ITimeData
{
    public async Task<bool> ExistsAsync(string courseId, string playerId)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("TimeExists", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new MySqlParameter("CourseId", courseId));
        command.Parameters.Add(new MySqlParameter("PlayerId", playerId));

        await connection.OpenAsync().ConfigureAwait(false);
        return await command.ExecuteBoolAsync().ConfigureAwait(false);
    }

    public async Task InsertAsync(Time time)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("TimeInsert", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new MySqlParameter("CourseId", time.CourseId));
        command.Parameters.Add(new MySqlParameter("PlayerId", time.PlayerId));
        command.Parameters.Add(new MySqlParameter("TimeDate", time.Date?.ToString("o", CultureInfo.InvariantCulture)));
        command.Parameters.Add(new MySqlParameter("TimeId", time.Id));
        command.Parameters.Add(new MySqlParameter("TimeSpan", time.Span));

        await connection.OpenAsync().ConfigureAwait(false);
        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }
}