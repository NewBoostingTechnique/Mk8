using Mk8.MySql.Console.Locations.Countries;

namespace Mk8.MySql.Console.Locations;

internal static class DeployLocations
{
    internal static async Task ExecuteAsync(MySqlConnection connection)
    {
        System.Console.WriteLine("Deploying Locations...");
        await CountryDeploy.ExecuteAsync(connection);
        await RegionDeploy.ExecuteAsync(connection);
    }
}
