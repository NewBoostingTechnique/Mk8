namespace Mk8.MySql.Console.Syncs;

internal static class SyncDeploy
{
    internal static async Task ExecuteAsync(MySqlConnection connection)
    {
        System.Console.WriteLine("Deploying Syncs...");
        await Script.ExecuteAsync(connection, "Syncs/SyncTable.sql");
        await Script.ExecuteAsync(connection, "Syncs/SyncDetail.sql");
        await Script.ExecuteAsync(connection, "Syncs/SyncInsert.sql");
        await Script.ExecuteAsync(connection, "Syncs/SyncUpdate.sql");
    }
}
