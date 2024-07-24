namespace Mk8.MySql.Console.Logins;

internal static class LoginSeed
{
    internal static async Task ExecuteAsync(MySqlConnection connection)
    {
        System.Console.WriteLine("Seeding Logins...");
        await Script.ExecuteAsync(connection, "Logins/LoginSeed.sql");
    }
}