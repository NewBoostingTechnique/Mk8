namespace Mk8.MySql.Console.Imports;

internal static class DeployImports
{
    internal static async Task ExecuteAsync(MySqlConnection connection)
    {
        System.Console.WriteLine("Deploying Imports...");
        await Script.ExecuteAsync(connection, "Imports/Imports.sql");
        await Script.ExecuteAsync(connection, "Imports/DetailImport.sql");
        await Script.ExecuteAsync(connection, "Imports/InsertImport.sql");
        await Script.ExecuteAsync(connection, "Imports/UpdateImport.sql");
    }
}
