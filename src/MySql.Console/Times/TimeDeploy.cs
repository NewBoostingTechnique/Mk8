namespace Mk8.MySql.Console.Times;

internal static class DeployTimes
{
    internal static async Task ExecuteAsync(MySqlConnection connection)
    {
        System.Console.WriteLine("Deploying Times...");
        await Script.ExecuteAsync(connection, "Times/TimeTable.sql");
        await Script.ExecuteAsync(connection, "Times/TimeExists.sql");
        await Script.ExecuteAsync(connection, "Times/TimeInsert.sql");
    }
}
