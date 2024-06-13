namespace Mk8.MySql.Console.ProofTypes;

internal static class ProofTypeDeploy
{
    internal static async Task ExecuteAsync(MySqlConnection connection)
    {
        System.Console.WriteLine("Deploying Proof Types...");
        await Script.ExecuteAsync(connection, "ProofTypes/ProofTypeTable.sql");
        await Script.ExecuteAsync(connection, "ProofTypes/ProofTypeExists.sql");
        await Script.ExecuteAsync(connection, "ProofTypes/ProofTypeIdentify.sql");
        await Script.ExecuteAsync(connection, "ProofTypes/ProofTypeList.sql");
    }
}
