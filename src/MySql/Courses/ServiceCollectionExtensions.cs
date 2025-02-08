using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Courses;

namespace Mk8.MySql.Courses;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddCourses(this IServiceCollection services)
    {
        return services.AddSingleton<ICourseStore, MySqlCourseStore>();
    }
}
