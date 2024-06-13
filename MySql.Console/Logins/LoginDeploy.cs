namespace Mk8.MySql.Console.Logins;

internal static class LoginDeploy
{
    internal static async Task ExecuteAsync(MySqlConnection connection)
    {
        System.Console.WriteLine("Deploying Logins...");
        await Script.ExecuteAsync(connection, "Logins/LoginTable.sql");
        await Script.ExecuteAsync(connection, "Logins/LoginExists.sql");
    }
}