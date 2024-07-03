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
        string? user = Environment.GetEnvironmentVariable("MYSQL_USER");

        while (user is null)
            user = getUserFromConsole();

        return user;

        static string? getUserFromConsole()
        {
            System.Console.Write("Login: ");
            return System.Console.ReadLine();
        }
    }

    private static string GetPassword() => GetPassword("MYSQL_PASSWORD");

    private static string GetPassword(string environmentVariableName)
    {
        string? password = Environment.GetEnvironmentVariable(environmentVariableName);

        if (password is null)
        {
            System.Console.Error.WriteLine($"Environment variable '{environmentVariableName}' is not set.");
            Environment.Exit(ExitCodes.MissingEnvironmentVariable);
        }

        return password;
    }

    private static string GetServer()
    {
        string? server = Environment.GetEnvironmentVariable("MYSQL_SERVER");

        while (server is null)
            server = getServerFromConsole();

        return server;

        static string? getServerFromConsole()
        {
            System.Console.Write("MK8 DB Server: ");
            return System.Console.ReadLine();
        }
    }

    private static string GetMk8Database()
    {
        string? mk8Database = Environment.GetEnvironmentVariable("MK8_DATABASE");

        while (mk8Database is null)
            mk8Database = getMk8DatabaseFromConsole();

        return mk8Database;

        static string? getMk8DatabaseFromConsole()
        {
            System.Console.Write("MK8 DB: ");
            return System.Console.ReadLine();
        }
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
