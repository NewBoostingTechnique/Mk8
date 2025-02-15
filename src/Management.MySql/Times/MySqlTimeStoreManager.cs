using Microsoft.Extensions.Options;
using Mk8.Management.Core;

namespace Mk8.Management.MySql.Times;

internal class MySqlTimeStoreManager(
    IOptions<MySqlManagementSettings> options
) : IStoreManager
{
    public async Task DeployAsync(string deploymentName, CancellationToken cancellationToken = default)
    {
        string targetConnectionString = options.Value.GetTargetConnectionString(deploymentName);

        await StoreManagerAssistant.ExecuteScriptFile
        (
            targetConnectionString,
            "Times/TimeTable.sql",
            cancellationToken
        );
    }
}
