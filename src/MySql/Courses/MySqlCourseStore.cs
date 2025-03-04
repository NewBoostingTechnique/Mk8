using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Mk8.Core.Courses;
using MySql.Data.MySqlClient;

namespace Mk8.MySql.Courses;

internal class MySqlCourseStore(
    IOptions<MySqlSettings> options
) : ICourseStore
{
    public async Task CreateAsync(Course course, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(course.Id);

        using MySqlConnection connection = new(options.Value.ConnectionString);

        using MySqlCommand command = new("course_create", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Id", course.Id);
        command.AddParameter("Name", course.Name);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<bool> ExistsAsync(string courseName, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(options.Value.ConnectionString);

        using MySqlCommand command = new("course_exists", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Name", courseName);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        return await command.ExecuteBoolAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<Ulid?> IdentifyAsync(string courseName, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(options.Value.ConnectionString);

        using MySqlCommand command = new("course_identify", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Name", courseName);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        return await command.ExecuteUlidAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<ImmutableList<Course>> IndexAsync(CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(options.Value.ConnectionString);

        using MySqlCommand command = new("course_index", connection);
        command.CommandType = CommandType.StoredProcedure;

        ImmutableList<Course>.Builder builder = ImmutableList.CreateBuilder<Course>();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            builder.Add(new Course
            {
                Name = reader.GetString("Name")
            });
        }
        return builder.ToImmutable();
    }
}
