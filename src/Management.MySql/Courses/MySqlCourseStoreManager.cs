using System.Reflection;
using Microsoft.Extensions.Options;
using Mk8.Management.Core;
using Mk8.Management.Core.Deployments;
using MySql.Data.MySqlClient;

namespace Mk8.Management.MySql.Courses;

internal class MySqlCourseStoreManager(
    StoreManagerAssistant assistant
) : IStoreManager
{
    public async Task DeployAsync(Deployment deployment, CancellationToken cancellationToken = default)
    {
        string targetConnectionString = assistant.GetTargetConnectionString(deployment);

        await StoreManagerAssistant.ExecuteScriptFile
        (
            targetConnectionString,
            // TODO: Break these 'table' scripts apart into individual pieces.
            // We may be able to run some in parallel.
            "Courses/CourseTable.sql",
            cancellationToken
        );
    }
}
