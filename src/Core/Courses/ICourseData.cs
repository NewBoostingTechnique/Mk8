using System.Collections.Immutable;

namespace Mk8.Core.Courses;

public interface ICourseData
{
    Task<bool> ExistsAsync(string courseName);

    Task<string?> IdentifyAsync(string courseName);

    Task<IImmutableList<Course>> ListAsync();
}