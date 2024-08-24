namespace Mk8.MySql.Console.News;

internal static class DeployNews
{
    internal static async Task ExecuteAsync(MySqlConnection connection)
    {
        System.Console.WriteLine("Deploying News...");
        await Script.ExecuteAsync(connection, "News/NewTable.sql");
        await Script.ExecuteAsync(connection, "News/NewClear.sql");
        await Script.ExecuteAsync(connection, "News/NewInsert.sql");
        await Script.ExecuteAsync(connection, "News/NewList.sql");
    }
}
