using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Mk8.Core;
using Mk8.Core.Courses;
using MySql.Data.MySqlClient;

namespace Mk8.MySql.Courses;

internal class MySqlCourseData(IOptions<Mk8Settings> mk8Options) : ICourseData
{
    public async Task CreateAsync(Course course)
    {
        ArgumentNullException.ThrowIfNull(course.Id);

        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("course_create", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Id", course.Id);
        command.AddParameter("Name", course.Name);

        await connection.OpenAsync().ConfigureAwait(false);
        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }

    public async Task<bool> ExistsAsync(string courseName)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("course_exists", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Name", courseName);

        await connection.OpenAsync().ConfigureAwait(false);
        return await command.ExecuteBoolAsync().ConfigureAwait(false);
    }

    public async Task<Ulid?> IdentifyAsync(string courseName)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("course_identify", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Name", courseName);

        await connection.OpenAsync().ConfigureAwait(false);
        return await command.ExecuteUlidAsync().ConfigureAwait(false);
    }

    public async Task<IImmutableList<Course>> IndexAsync()
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("course_index", connection);
        command.CommandType = CommandType.StoredProcedure;

        ImmutableList<Course>.Builder builder = ImmutableList.CreateBuilder<Course>();
        await connection.OpenAsync().ConfigureAwait(false);
        using DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
        while (await reader.ReadAsync().ConfigureAwait(false))
        {
            builder.Add(new Course
            {
                Name = reader.GetString("Name")
            });
        }
        return builder.ToImmutable();
    }
}
