using System.Data;
using Microsoft.Extensions.Options;
using Mk8.Core;
using Mk8.Core.Times;
using MySql.Data.MySqlClient;

namespace Mk8.MySql.Times;

internal class MySqlTimeStore(IOptions<Mk8Settings> mk8Options) : ITimeStore
{
    public async Task CreateAsync(Time time)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("time_create", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Id", time.Id);
        command.AddParameter("CourseId", time.CourseId);
        command.AddParameter("PlayerId", time.PlayerId);
        command.AddParameter("TimeDate", time.Date);
        command.AddParameter("TimeSpan", time.Span);

        await connection.OpenAsync().ConfigureAwait(false);
        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }

    public async Task<bool> ExistsAsync(Ulid courseId, Ulid playerId)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("time_exists", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("CourseId", courseId);
        command.AddParameter("PlayerId", playerId);

        await connection.OpenAsync().ConfigureAwait(false);
        return await command.ExecuteBoolAsync().ConfigureAwait(false);
    }
}
