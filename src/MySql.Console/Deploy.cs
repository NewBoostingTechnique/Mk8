using Microsoft.Extensions.Options;
using Mk8.Core;
using Mk8.MySql.Console.Courses;
using Mk8.MySql.Console.Locations;
using Mk8.MySql.Console.Logins;
using Mk8.MySql.Console.News;
using Mk8.MySql.Console.Persons;
using Mk8.MySql.Console.Players;
using Mk8.MySql.Console.ProofTypes;
using Mk8.MySql.Console.Syncs;
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

        await CourseDeploy.ExecuteAsync(mk8Connection);
        await LocationDeploy.ExecuteAsync(mk8Connection);
        await PersonDeploy.ExecuteAsync(mk8Connection);
        await ProofTypeDeploy.ExecuteAsync(mk8Connection);
        await PlayerDeploy.ExecuteAsync(mk8Connection);
        await TimeDeploy.ExecuteAsync(mk8Connection);
        await LoginDeploy.ExecuteAsync(mk8Connection);
        await NewDeploy.ExecuteAsync(mk8Connection);
        await SyncDeploy.ExecuteAsync(mk8Connection);
    }
}
