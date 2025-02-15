using Microsoft.Extensions.Options;
using Mk8.Management.Core.Deployments;
using MySql.Data.MySqlClient;

namespace Mk8.Management.MySql.Deployments;

internal class MySqlDeploymentStore(
    IOptions<MySqlManagementSettings> options
) : IDeploymentStore
{
    public async Task CreateAsync(Deployment deployment, CancellationToken cancellationToken = default)
    {
        // Require the target and root connection strings to be on the same server.
        MySqlConnectionStringBuilder rootConnectionStringBuilder = new(options.Value.RootConnectionString);
        MySqlConnectionStringBuilder targetConnectionStringBuilder = new(options.Value.GetTargetConnectionString(deployment.Name));
        if (targetConnectionStringBuilder.Server != rootConnectionStringBuilder.Server)
            throw new InvalidOperationException("The target connection string must point a the same server as the root connection string.");

        // Connect to the database server as root.
        using MySqlConnection rootConnection = new(rootConnectionStringBuilder.ConnectionString);
        await rootConnection.OpenAsync(cancellationToken).ConfigureAwait(false);

        // Create the target database.
        using MySqlCommand command = new($"CREATE DATABASE IF NOT EXISTS {targetConnectionStringBuilder.Database};", rootConnection);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);

        // Create the target database user.
        command.CommandText = $"CREATE USER IF NOT EXISTS '{targetConnectionStringBuilder.UserID}'@'%%' IDENTIFIED BY '{targetConnectionStringBuilder.Password}';";
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        command.CommandText = $"USE {targetConnectionStringBuilder.Database}; GRANT EXECUTE ON * TO 'mk8'@'%%';";
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }
}
