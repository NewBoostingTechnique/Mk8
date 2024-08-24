using Microsoft.Extensions.Options;
using Mk8.Core;
using Mk8.MySql.Console.Courses;
using Mk8.MySql.Console.Imports;
using Mk8.MySql.Console.Locations;
using Mk8.MySql.Console.Logins;
using Mk8.MySql.Console.News;
using Mk8.MySql.Console.Persons;
using Mk8.MySql.Console.Players;
using Mk8.MySql.Console.Times;

namespace Mk8.MySql.Console;

// TODO: Use GitHub issues instead of 'TODO's.

internal class DeployCommand(IOptions<Mk8Settings> options)
{
    internal async Task ExecuteAsync()
    {
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

        await DeployCourses.ExecuteAsync(mk8Connection);
        await DeployLocations.ExecuteAsync(mk8Connection);
        await DeployPersons.ExecuteAsync(mk8Connection);
        await DeployPlayers.ExecuteAsync(mk8Connection);
        await DeployTimes.ExecuteAsync(mk8Connection);
        await DeployLogins.ExecuteAsync(mk8Connection);
        await DeployNews.ExecuteAsync(mk8Connection);
        await DeployImports.ExecuteAsync(mk8Connection);
    }
}
