namespace Mk8.MySql.Console.Persons;

internal static class PersonSeed
{
    internal static async Task ExecuteAsync(MySqlConnection connection)
    {
        System.Console.WriteLine("Seeding Persons...");
        await Script.ExecuteAsync(connection, "Persons/PersonSeed.sql");
    }
}
