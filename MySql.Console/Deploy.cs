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

// TODO: app user and root user currently have same password.

// TODO: Use GitHub issues instead of 'TODO's.

internal static partial class Program
{
    private static async Task DeployAsync()
    {
        string user = GetUser();
        string password = GetPassword();
        string server = GetServer();
        string mk8Database = GetMk8Database();
        string mk8Password = GetMk8Password();

        using MySqlConnection rootConnection = new($"Server={server};Uid={user};Pwd={password}");
        await rootConnection.OpenAsync();

        using MySqlCommand command = new($"CREATE DATABASE IF NOT EXISTS {mk8Database};", rootConnection);
        await command.ExecuteNonQueryAsync();

        command.CommandText = $"CREATE USER IF NOT EXISTS 'mk8'@'%%' IDENTIFIED BY '{mk8Password}';";
        await command.ExecuteNonQueryAsync();

        command.CommandText = $"USE {mk8Database}; GRANT EXECUTE ON * TO 'mk8'@'%%';";
        await command.ExecuteNonQueryAsync();

        using MySqlConnection mk8Connection = await GetMk8ConnectionAsync(user, password, server, mk8Database);

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

    private static string GetMk8Password() => GetPassword("MK8_PASSWORD");
}
