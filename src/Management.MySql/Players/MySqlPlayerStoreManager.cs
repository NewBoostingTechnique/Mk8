using Mk8.Management.Core;
using Mk8.Management.Core.Deployments;

namespace Mk8.Management.MySql.Players;

internal class MySqlPlayerStoreManager(
    StoreManagerAssistant assistant
) : IStoreManager
{
    public async Task DeployAsync(Deployment deployment, CancellationToken cancellationToken = default)
    {
        // TODO: Maybe the target connection string should be passed in.
        string targetConnectionString = assistant.GetTargetConnectionString(deployment);

        await StoreManagerAssistant.ExecuteScriptFile
        (
            targetConnectionString,
            "Players/PlayerTable.sql",
            cancellationToken
        );
    }
}
