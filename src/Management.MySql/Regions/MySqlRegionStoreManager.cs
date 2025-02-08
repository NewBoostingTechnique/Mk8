using System.Reflection;
using Microsoft.Extensions.Options;
using Mk8.Management.Core;
using Mk8.Management.Core.Deployments;
using MySql.Data.MySqlClient;

namespace Mk8.Management.MySql.Regions;

internal class MySqlRegionStoreManager(
    StoreManagerAssistant assistant
) : IStoreManager
{
    public async Task DeployAsync(Deployment deployment, CancellationToken cancellationToken = default)
    {
        string targetConnectionString = assistant.GetTargetConnectionString(deployment);

        await StoreManagerAssistant.ExecuteScriptFile
        (
            targetConnectionString,
            "Regions/RegionTable.sql",
            cancellationToken
        );
    }
}
