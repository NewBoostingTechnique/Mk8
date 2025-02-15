using Microsoft.Extensions.Options;
using Mk8.Management.Core;

namespace Mk8.Management.MySql.Courses;

internal class MySqlCourseStoreManager(
    IOptions<MySqlManagementSettings> options
) : IStoreManager
{
    public async Task DeployAsync(string deploymentName, CancellationToken cancellationToken = default)
    {
        string targetConnectionString = options.Value.GetTargetConnectionString(deploymentName);

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
