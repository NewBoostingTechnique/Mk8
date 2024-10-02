using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mk8.Core;

namespace Mk8.MySql.Console;

// TODO: Use GitHub issues instead of 'TODO's.

internal class DeployCommand(
    ILogger<DeployCommand> logger,
    IOptions<Mk8Settings> options
)
{
    internal async Task ExecuteAsync()
    {
        Assembly assembly = Assembly.GetEntryAssembly()
            ?? throw new InvalidOperationException("Failed to get the entry assembly while determining the path to SQL scripts.");

        MySqlConnectionStringBuilder mk8ConnectionString = new(options.Value.ConnectionString);

        using MySqlConnection rootConnection = new(options.Value.RootConnectionString);
        await rootConnection.OpenAsync();

        using MySqlCommand command = new($"CREATE DATABASE IF NOT EXISTS {mk8ConnectionString.Database};", rootConnection);
        await command.ExecuteNonQueryAsync();

        command.CommandText = $"CREATE USER IF NOT EXISTS 'mk8'@'%%' IDENTIFIED BY '{mk8ConnectionString.Password}';";
        await command.ExecuteNonQueryAsync();

        command.CommandText = $"USE {mk8ConnectionString.Database}; GRANT EXECUTE ON * TO 'mk8'@'%%';";
        await command.ExecuteNonQueryAsync();

        using MySqlConnection mk8Connection = new(options.Value.ConnectionString);
        await mk8Connection.OpenAsync();

        await deployTable("course", "Courses/CourseTable.sql");
        await deployTable("country", "Countries/CountryTable.sql");
        await deployTable("region", "Regions/RegionTable.sql");
        await deployTable("person", "Persons/PersonTable.sql");
        await deployTable("player", "Players/PlayerTable.sql");
        await deployTable("time", "Times/TimeTable.sql");
        await deployTable("login", "Logins/LoginTable.sql");
        await deployTable("new", "News/NewTable.sql");
        await deployTable("migration", "Migrations/MigrationTable.sql");

        async Task deployTable(string name, string scriptPath)
        {
            logger.LogInformation("Deploying '{Name}' table.", name);

            using MySqlCommand command = new
            (
                await File.ReadAllTextAsync
                (
                    Path.Combine
                    (
                        Path.GetDirectoryName(assembly.Location)!,
                        scriptPath
                    )
                ),
                mk8Connection
            );

            await command.ExecuteNonQueryAsync();
        }
    }
}
