namespace Mk8.MySql.Console.Locations.Countries;

internal static class CountrySeed
{
    internal static async Task ExecuteAsync(MySqlConnection connection)
    {
        System.Console.WriteLine("Seeding Countries...");
        await Script.ExecuteAsync(connection, "Locations/Countries/CountrySeed.sql");
    }
}
