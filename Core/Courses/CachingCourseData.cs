using System.Collections.Immutable;
using Microsoft.Extensions.Caching.Memory;

namespace Mk8.Core.Courses;

internal class CachingCourseData(IMemoryCache cache, ICourseData innerData) : ICourseData
{
    public Task<bool> ExistsAsync(string courseName)
    {
        return cache.GetOrCreateAsync
        (
            $"Course_Exists:{courseName}",
            entry => innerData.ExistsAsync(courseName)
        );
    }

    public Task<string?> IdentifyAsync(string courseName)
    {
        return cache.GetOrCreateAsync
        (
            $"Course_Identify:{courseName}",
            entry => innerData.IdentifyAsync(courseName)
        );
    }

    public Task<IImmutableList<Course>> ListAsync()
    {
        return cache.GetOrCreateAsync
        (
            "Course_List",
            entry => innerData.ListAsync()
        )!;
    }
}