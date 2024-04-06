using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Courses.MySql;
using Mk8.Core.Extensions;

namespace Mk8.Core.Courses;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddCourses(this IServiceCollection services)
    {
        return services
            .AddSingleton<ICourseData, MySqlCourseData>()
            .AddCourseCaching()
            .AddSingleton<ICourseService, CourseService>();
    }

    private static IServiceCollection AddCourseCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        ServiceDescriptor? descriptor = services.GetServiceDescriptor<ICourseData>();

        return services.AddSingleton<ICourseData>
        (
            sp => ActivatorUtilities.CreateInstance<CachingCourseData>
            (
                sp,
                (ICourseData)descriptor.CreateInstance(sp)
            )
        );
    }
}