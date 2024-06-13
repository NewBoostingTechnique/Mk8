namespace Mk8.MySql.Console;

internal static partial class RegionSeed
{
    internal static async Task ExecuteAsync(MySqlConnection connection)
    {
        System.Console.WriteLine("Seeding Regions...");
        await Script.ExecuteAsync(connection, "Locations/Regions/RegionSeed.sql");
    }
}
