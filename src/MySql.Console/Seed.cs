using Mk8.MySql.Console.Courses;
using Mk8.MySql.Console.Locations;
using Mk8.MySql.Console.Logins;
using Mk8.MySql.Console.Persons;
using Mk8.MySql.Console.ProofTypes;

namespace Mk8.MySql.Console;

internal static partial class Program
{
    private static async Task SeedAsync()
    {
        string user = GetUser();
        string password = GetPassword();
        string server = GetServer();
        string database = GetMk8Database();

        using MySqlConnection connection = await GetMk8ConnectionAsync(user, password, server, database);

        await CourseSeed.ExecuteAsync(connection);
        await LocationSeed.ExecuteAsync(connection);
        await PersonSeed.ExecuteAsync(connection);
        await ProofTypeSeed.ExecuteAsync(connection);
        await LoginSeed.ExecuteAsync(connection);
    }
}