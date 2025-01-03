using System.Collections.Immutable;

namespace Mk8.Core.Courses;

public interface ICourseStore
{
    Task CreateAsync(Course course, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(string courseName, CancellationToken cancellationToken = default);

    Task<Ulid?> IdentifyAsync(string courseName, CancellationToken cancellationToken = default);

    Task<ImmutableList<Course>> IndexAsync(CancellationToken cancellationToken = default);
}
