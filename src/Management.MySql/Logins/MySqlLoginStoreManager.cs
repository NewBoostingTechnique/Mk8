using Microsoft.Extensions.Options;
using Mk8.Management.Core;
using Mk8.Management.Core.Deployments;

namespace Mk8.Management.MySql.Logins;

internal class MySqlLoginStoreManager(
    IOptions<MySqlManagementSettings> options
) : IStoreManager
{
    public async Task DeployAsync(string deploymentName, CancellationToken cancellationToken = default)
    {
        string targetConnectionString = options.Value.GetTargetConnectionString(deploymentName);

        await StoreManagerAssistant.ExecuteScriptFile
        (
            targetConnectionString,
            "Logins/LoginTable.sql",
            cancellationToken
        );
    }
}
