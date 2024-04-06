using System.Collections.Immutable;

namespace Mk8.Core.Courses;

internal class CourseService(ICourseData courseData) : ICourseService
{
    public Task<bool> ExistsAsync(string courseName)
    {
        return courseData.ExistsAsync(courseName);
    }

    public Task<IImmutableList<Course>> ListAsync()
    {
        return courseData.ListAsync();
    }
}