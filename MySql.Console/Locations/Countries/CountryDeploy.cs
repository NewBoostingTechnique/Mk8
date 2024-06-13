namespace Mk8.MySql.Console.Locations.Countries;

internal static class CountryDeploy
{
    internal static async Task ExecuteAsync(MySqlConnection connection)
    {
        System.Console.WriteLine("Deploying Countries...");
        await Script.ExecuteAsync(connection, "Locations/Countries/CountryTable.sql");
        await Script.ExecuteAsync(connection, "Locations/Countries/CountryIdentify.sql");
        await Script.ExecuteAsync(connection, "Locations/Countries/CountryList.sql");
    }
}
