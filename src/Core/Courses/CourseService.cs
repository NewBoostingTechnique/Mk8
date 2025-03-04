using System.Collections.Immutable;

namespace Mk8.Core.Courses;

internal class CourseService(
    ICourseStore courseStore
) : ICourseService
{
    public Task<bool> ExistsAsync(string courseName, CancellationToken cancellationToken = default)
    {
        return courseStore.ExistsAsync(courseName, cancellationToken);
    }

    public Task<ImmutableList<Course>> IndexAsync(CancellationToken cancellationToken = default)
    {
        return courseStore.IndexAsync(cancellationToken);
    }
}
