using System.Reflection;
using MySql.Data.MySqlClient;

namespace Mk8.Management.MySql;

internal static class StoreManagerAssistant
{
    private static readonly Assembly _assembly = Assembly.GetEntryAssembly()
        ?? throw new InvalidOperationException("Failed to get the entry assembly while determining the path to SQL scripts.");

    internal static async Task ExecuteScriptFile(
        string connectionString,
        string scriptPath,
        CancellationToken cancellationToken = default
    )
    {
        using MySqlConnection connection = new(connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using MySqlCommand command = new
        (
            await File.ReadAllTextAsync
            (
                Path.Combine
                (
                    Path.GetDirectoryName(_assembly.Location)!,
                    scriptPath
                ),
                cancellationToken
            )
            .ConfigureAwait(false),
            connection
        );

        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }
}
