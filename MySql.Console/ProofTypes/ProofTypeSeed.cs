namespace Mk8.MySql.Console.ProofTypes;

internal static class ProofTypeSeed
{
    internal static async Task ExecuteAsync(MySqlConnection connection)
    {
        System.Console.WriteLine("Seeding Proof Types...");
        await Script.ExecuteAsync(connection, "ProofTypes/ProofTypeSeed.sql");
    }
}
