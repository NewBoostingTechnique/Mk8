namespace Mk8.MySql.Console.Persons;

internal static class PersonDeploy
{
    internal static async Task ExecuteAsync(MySqlConnection connection)
    {
        System.Console.WriteLine("Deploying Persons...");
        await Script.ExecuteAsync(connection, "Persons/PersonTable.sql");
        await Script.ExecuteAsync(connection, "Persons/PersonIdentify.sql");
        await Script.ExecuteAsync(connection, "Persons/PersonInsert.sql");
    }
}
