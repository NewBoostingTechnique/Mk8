
using System.Data;
using Microsoft.Extensions.Options;
using Mk8.Core.Extensions;
using Mk8.Core.MySql;
using MySql.Data.MySqlClient;

namespace Mk8.Core.Times.MySql;

internal class MySqlTimeData(IOptions<Mk8Settings> mk8Options) : ITimeData
{
    public async Task<bool> ExistsAsync(string courseId, string playerId)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("TimeExists", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("CourseId", courseId);
        command.AddParameter("PlayerId", playerId);

        await connection.OpenAsync().ConfigureAwait(false);
        return await command.ExecuteBoolAsync().ConfigureAwait(false);
    }

    public async Task InsertAsync(Time time)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("TimeInsert", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("CourseId", time.CourseId);
        command.AddParameter("PlayerId", time.PlayerId);
        command.AddParameter("TimeDate", time.Date);
        command.AddParameter("TimeId", time.Id);
        command.AddParameter("TimeSpan", time.Span);

        await connection.OpenAsync().ConfigureAwait(false);
        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }
}