using System.Data;
using Microsoft.Extensions.Options;
using Mk8.Core.Times;
using MySql.Data.MySqlClient;

namespace Mk8.MySql.Times;

internal class MySqlTimeStore(
    IOptions<MySqlSettings> options
) : ITimeStore
{
    public async Task CreateAsync(Time time, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(options.Value.ConnectionString);

        using MySqlCommand command = new("time_create", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Id", time.Id);
        command.AddParameter("Span", time.Span);
        command.AddParameter("Date", time.Date);
        command.AddParameter("CourseId", time.CourseId);
        command.AddParameter("PlayerId", time.PlayerId);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<bool> ExistsAsync(Ulid courseId, Ulid playerId, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(options.Value.ConnectionString);

        using MySqlCommand command = new("time_exists", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("CourseId", courseId);
        command.AddParameter("PlayerId", playerId);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        return await command.ExecuteBoolAsync(cancellationToken).ConfigureAwait(false);
    }
}
