using Microsoft.Extensions.Options;
using Mk8.Management.Core;

namespace Mk8.Management.MySql.Regions;

internal class MySqlRegionStoreManager(
    IOptions<MySqlManagementSettings> options
) : IStoreManager
{
    public async Task DeployAsync(string deploymentName, CancellationToken cancellationToken = default)
    {
        string targetConnectionString = options.Value.GetTargetConnectionString(deploymentName);

        await StoreManagerAssistant.ExecuteScriptFile
        (
            targetConnectionString,
            "Regions/RegionTable.sql",
            cancellationToken
        );
    }
}
