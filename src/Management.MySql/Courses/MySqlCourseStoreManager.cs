using Microsoft.Extensions.Options;
using Mk8.Management.Core;
using MySql.Data.MySqlClient;

namespace Mk8.Management.MySql.Courses;

internal class MySqlCourseStoreManager(
    IOptions<MySqlManagementSettings> options
) : IStoreManager
{
    public async Task DeployAsync(string deploymentName, CancellationToken cancellationToken = default)
    {
        string targetConnectionString = new MySqlConnectionStringBuilder(options.Value.RootConnectionString)
        {
            Database = deploymentName
        }
        .ConnectionString;

        await StoreManagerAssistant.ExecuteScriptFile
        (
            targetConnectionString,
            "Courses/CourseTable.sql",
            cancellationToken
        );
    }
}
