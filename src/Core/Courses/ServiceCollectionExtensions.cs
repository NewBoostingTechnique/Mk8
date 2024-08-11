using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.DependencyInjection;

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

        services.AddSingleton(sp => ActivatorUtilities.CreateInstance<EventingCourseData>
        (
            sp,
            (ICourseData)descriptor.CreateInstance(sp)
        ));

        services.AddSingleton<ICourseDataEvents>(sp => sp.GetRequiredService<EventingCourseData>());

        services.AddSingleton<ICourseData>
        (
            sp =>
            {
                EventingCourseData eventingCourseData = sp.GetRequiredService<EventingCourseData>();
                return ActivatorUtilities.CreateInstance<CachingCourseData>
                (
                    sp,
                    eventingCourseData,
                    eventingCourseData
                );
            }
        );
    }
}
