using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Extensions;

namespace Mk8.Core.Courses;

internal static class ServiceCollectionExtensions
{
    internal static void AddCourses(this IServiceCollection services)
    {
        services.AddCourseCaching();
        services.AddSingleton<ICourseService, CourseService>();
    }

    private static void AddCourseCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        ServiceDescriptor? descriptor = services.GetServiceDescriptor<ICourseData>()
            ?? throw new InvalidOperationException("An implementation of ICourseData must be registered before calling AddCourseCaching.");

        services.AddSingleton<ICourseData>
        (
            sp => ActivatorUtilities.CreateInstance<CachingCourseData>
            (
                sp,
                (ICourseData)descriptor.CreateInstance(sp)
            )
        );
    }
}