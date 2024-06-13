using System.CommandLine;

namespace Mk8.MySql.Console;

internal static partial class Program
{
    private static async Task Main(string[] args)
    {
        RootCommand rootCommand = new("MK8 MySQL Console");

        // TODO: Make 'help' the default command.
        rootCommand.SetHandler(DeployAsync);

        Command deploy = new("deploy");
        deploy.SetHandler(DeployAsync);
        rootCommand.Add(deploy);

        Command seed = new("seed");
        seed.SetHandler(SeedAsync);
        rootCommand.Add(seed);

        await rootCommand.InvokeAsync(args);
    }

    private static string GetUser()
    {
        // TODO: Set to USER variable if not already set.
        System.Console.Write("Login: ");
        return System.Console.ReadLine();
    }

    private static string GetPassword()
    {
        // TODO: Require that this is provided as an environment variable.
        System.Console.Write("Password: ");
        return System.Console.ReadLine();
    }

    private static string GetServer()
    {
        // TODO: Use command line arguments
        System.Console.Write("MK8 DB Server: ");
        return System.Console.ReadLine();
    }

    private static string GetMk8Database()
    {
        // TODO: Use command line arguments
        System.Console.Write("MK8 DB: ");
        return System.Console.ReadLine();
    }

    private static async Task<MySqlConnection> GetMk8ConnectionAsync(
        string user,
        string password,
        string server,
        string mk8Database
    )
    {
        MySqlConnection mk8Connection = new($"Server={server};Database={mk8Database};Uid={user};Pwd={password}");
        await mk8Connection.OpenAsync();
        return mk8Connection;
    }
}
