using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mk8.Core;

namespace Mk8.MySql.Console;

internal class DeployCommand(
    ILogger<DeployCommand> logger,
    IOptions<Mk8Settings> options
)
{
    internal async Task ExecuteAsync()
    {
        Assembly assembly = Assembly.GetEntryAssembly()
            ?? throw new InvalidOperationException("Failed to get the entry assembly while determining the path to SQL scripts.");

        // Require the mk8 and root connection strings to be on the same server.
        MySqlConnectionStringBuilder mk8ConnectionString = new(options.Value.ConnectionString);
        MySqlConnectionStringBuilder rootConnectionString = new(options.Value.RootConnectionString);
        if (mk8ConnectionString.Server != rootConnectionString.Server)
            throw new InvalidOperationException("The root and MK8 connection strings must have the same server.");

        // Connect to the db server as root.
        using MySqlConnection rootConnection = new(options.Value.RootConnectionString);
        await rootConnection.OpenAsync();

        // Create the mk8 database.
        using MySqlCommand command = new($"CREATE DATABASE IF NOT EXISTS {mk8ConnectionString.Database};", rootConnection);
        await command.ExecuteNonQueryAsync();

        // Create the mk8 app user.
        command.CommandText = $"CREATE USER IF NOT EXISTS '{mk8ConnectionString.UserID}'@'%%' IDENTIFIED BY '{mk8ConnectionString.Password}';";
        await command.ExecuteNonQueryAsync();
        command.CommandText = $"USE {mk8ConnectionString.Database}; GRANT EXECUTE ON * TO 'mk8'@'%%';";
        await command.ExecuteNonQueryAsync();

        // Connect to the mk8 database as the root user (to deploy the schema).
        mk8ConnectionString.UserID = rootConnectionString.UserID;
        mk8ConnectionString.Password = rootConnectionString.Password;
        using MySqlConnection mk8Connection = new(mk8ConnectionString.ConnectionString);
        await mk8Connection.OpenAsync();

        // Deploy the schema.
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
