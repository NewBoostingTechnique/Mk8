using Microsoft.Extensions.Options;
using Mk8.Management.Core.Deployments.Delete.Storage;
using Mk8.Management.MySql.Deployments.Delete.Storage;
using MySql.Data.MySqlClient;

namespace Mk8.Management.MySql.Deployments.Storage.Delete;

internal class MySqlDeleteDeploymentStorage(
    IOptions<MySqlManagementSettings> options
) : IDeleteDeploymentStorage
{
    public async Task DeleteDeploymentAsync(DeleteDeploymentDto dto, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(options.Value.RootConnectionString);
        using MySqlCommand command = new($"drop database {dto.Name};", connection);
        await connection.OpenAsync(cancellationToken);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
