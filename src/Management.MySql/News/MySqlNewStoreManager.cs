using Microsoft.Extensions.Options;
using Mk8.Management.Core;

namespace Mk8.Management.MySql.News;

internal class MySqlNewStoreManager(
    IOptions<MySqlManagementSettings> options
) : IStoreManager
{
    public async Task DeployAsync(string deploymentName, CancellationToken cancellationToken = default)
    {
        string targetConnectionString = options.Value.GetTargetConnectionString(deploymentName);

        await StoreManagerAssistant.ExecuteScriptFile
        (
            targetConnectionString,
            "News/NewTable.sql",
            cancellationToken
        );
    }
}
