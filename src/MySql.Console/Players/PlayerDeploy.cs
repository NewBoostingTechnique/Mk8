namespace Mk8.MySql.Console.Players;

internal static class DeployPlayers
{
    internal static async Task ExecuteAsync(MySqlConnection connection)
    {
        System.Console.WriteLine("Deploying Players...");
        await Script.ExecuteAsync(connection, "Players/PlayerTable.sql");
        await Script.ExecuteAsync(connection, "Players/PlayerDelete.sql");
        await Script.ExecuteAsync(connection, "Players/PlayerDetail.sql");
        await Script.ExecuteAsync(connection, "Players/PlayerExists.sql");
        await Script.ExecuteAsync(connection, "Players/PlayerIdentify.sql");
        await Script.ExecuteAsync(connection, "Players/PlayerInsert.sql");
        await Script.ExecuteAsync(connection, "Players/PlayerList.sql");
    }
}
