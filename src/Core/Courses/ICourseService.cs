using System.Collections.Immutable;

namespace Mk8.Core.Courses;

public interface ICourseService
{
    Task<bool> ExistsAsync(string courseName, CancellationToken cancellationToken = default);

    Task<ImmutableList<Course>> IndexAsync(CancellationToken cancellationToken = default);

    Task SeedAsync(CancellationToken cancellationToken = default);
}
