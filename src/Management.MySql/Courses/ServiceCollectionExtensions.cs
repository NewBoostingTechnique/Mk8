using Microsoft.Extensions.DependencyInjection;
using Mk8.Management.Core;

namespace Mk8.Management.MySql.Courses;

internal static class ServiceCollectionExtensions
{
    internal static void AddCourses(this IServiceCollection services)
    {
        services.AddSingleton<IStoreManager, MySqlCourseStoreManager>();
    }
}
