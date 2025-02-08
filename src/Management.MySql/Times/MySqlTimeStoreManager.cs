using Mk8.Management.Core;
using Mk8.Management.Core.Deployments;

namespace Mk8.Management.MySql.Times;

internal class MySqlTimeStoreManager(
    StoreManagerAssistant assistant
) : IStoreManager
{
    public async Task DeployAsync(Deployment deployment, CancellationToken cancellationToken = default)
    {
        string targetConnectionString = assistant.GetTargetConnectionString(deployment);

        await StoreManagerAssistant.ExecuteScriptFile
        (
            targetConnectionString,
            "Times/TimeTable.sql",
            cancellationToken
        );
    }
}
