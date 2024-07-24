using Mk8.MySql.Console.Locations.Countries;

namespace Mk8.MySql.Console.Locations;

internal static class LocationSeed
{
    internal static async Task ExecuteAsync(MySqlConnection connection)
    {
        System.Console.WriteLine("Seeding Locations...");
        await CountrySeed.ExecuteAsync(connection);
        await RegionSeed.ExecuteAsync(connection);
    }
}