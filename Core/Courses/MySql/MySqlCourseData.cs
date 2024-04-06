using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Mk8.Core.Extensions;
using MySql.Data.MySqlClient;

namespace Mk8.Core.Courses.MySql;

internal class MySqlCourseData(IOptions<Mk8Settings> mk8Options) : ICourseData
{
    public async Task<bool> ExistsAsync(string courseName)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("CourseExists", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new MySqlParameter("CourseName", courseName));

        await connection.OpenAsync().ConfigureAwait(false);
        return await command.ExecuteBoolAsync().ConfigureAwait(false);
    }

    public async Task<string?> IdentifyAsync(string courseName)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("CourseIdentify", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new MySqlParameter("CourseName", courseName));

        await connection.OpenAsync().ConfigureAwait(false);
        return await command.ExecuteScalarAsync().ConfigureAwait(false) as string;
    }

    public async Task<IImmutableList<Course>> ListAsync()
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("CourseList", connection);
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