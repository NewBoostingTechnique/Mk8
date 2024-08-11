namespace Mk8.MySql.Console;

internal static partial class RegionDeploy
{
    internal static async Task ExecuteAsync(MySqlConnection connection)
    {
        System.Console.WriteLine("Deploying Regions...");
        await Script.ExecuteAsync(connection, "Locations/Regions/RegionTable.sql");
        await Script.ExecuteAsync(connection, "Locations/Regions/RegionIdentify.sql");
        await Script.ExecuteAsync(connection, "Locations/Regions/RegionInsert.sql");
        await Script.ExecuteAsync(connection, "Locations/Regions/RegionList.sql");
    }
}
