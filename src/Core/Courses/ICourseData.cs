using System.Collections.Immutable;

namespace Mk8.Core.Courses;

public interface ICourseData
{
    Task<bool> ExistsAsync(string courseName);

    Task<Ulid?> IdentifyAsync(string courseName);

    Task InsertAsync(Course course);

    Task<IImmutableList<Course>> ListAsync();
}
