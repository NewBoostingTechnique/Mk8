using System.Reflection;
using Microsoft.Extensions.Options;
using Mk8.Management.Core.Deployments;
using MySql.Data.MySqlClient;

namespace Mk8.Management.MySql;

internal class StoreManagerAssistant(IOptions<MySqlManagementSettings> options)
{
    private static readonly Assembly _assembly = Assembly.GetEntryAssembly()
        ?? throw new InvalidOperationException("Failed to get the entry assembly while determining the path to SQL scripts.");

    // Connect to the target database as the root user (to deploy the schema).
    internal string GetTargetConnectionString(Deployment deployment)
    {
        // TODO: The target db name should come form the deployment parameter.

        MySqlConnectionStringBuilder rootConnectionStringBuilder = new(options.Value.RootConnectionString);
        MySqlConnectionStringBuilder targetConnectionStringBuilder = new(options.Value.ConnectionString)
        {
            UserID = rootConnectionStringBuilder.UserID,
            Password = rootConnectionStringBuilder.Password
        };
        return targetConnectionStringBuilder.ConnectionString;
    }

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
