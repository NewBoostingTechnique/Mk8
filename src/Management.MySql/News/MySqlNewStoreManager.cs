using Mk8.Management.Core;
using Mk8.Management.Core.Deployments;

namespace Mk8.Management.MySql.News;

internal class MySqlNewStoreManager(
    StoreManagerAssistant assistant
) : IStoreManager
{
    public async Task DeployAsync(Deployment deployment, CancellationToken cancellationToken = default)
    {
        string targetConnectionString = assistant.GetTargetConnectionString(deployment);

        await StoreManagerAssistant.ExecuteScriptFile
        (
            targetConnectionString,
            "News/NewTable.sql",
            cancellationToken
        );
    }
}
