using System.Reflection;

namespace Mk8.MySql.Console;

internal static class Script
{
    internal static async Task ExecuteAsync(MySqlConnection connection, string scriptPath)
    {
        Assembly? assembly = Assembly.GetEntryAssembly()
            ?? throw new InvalidOperationException("Failed to get the entry assembly while determining the path to SQL scripts.");

        using MySqlCommand command = new
        (
            await File.ReadAllTextAsync
            (
                Path.Combine
                (
                    Path.GetDirectoryName(assembly.Location)!,
                    scriptPath
                )
            ),
            connection
        );

        await command.ExecuteNonQueryAsync();
    }
}
