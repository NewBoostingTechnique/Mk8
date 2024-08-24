namespace Mk8.MySql.Console.Logins;

internal static class DeployLogins
{
    internal static async Task ExecuteAsync(MySqlConnection connection)
    {
        System.Console.WriteLine("Deploying Logins...");
        await Script.ExecuteAsync(connection, "Logins/LoginTable.sql");
        await Script.ExecuteAsync(connection, "Logins/LoginExists.sql");
        await Script.ExecuteAsync(connection, "Logins/LoginInsert.sql");
    }
}
