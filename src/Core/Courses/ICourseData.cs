using System.Collections.Immutable;

namespace Mk8.Core.Courses;

public interface ICourseData
{
    Task CreateAsync(Course course);

    Task<bool> ExistsAsync(string courseName);

    Task<Ulid?> IdentifyAsync(string courseName);

    Task<IImmutableList<Course>> IndexAsync();
}
