using System.Collections.Immutable;

namespace Mk8.Core.Courses;

public interface ICourseService
{
    Task<bool> ExistsAsync(string courseName);

    Task<IImmutableList<Course>> ListAsync();
}