using System.Collections.Immutable;
using static Mk8.Core.Courses.ICourseDataEvents;

namespace Mk8.Core.Courses;

internal class EventingCourseData(ICourseStore innerStore)
    : ICourseStore, ICourseDataEvents
{

    public Task<bool> ExistsAsync(string courseName, CancellationToken cancellationToken = default)
    {
        return innerStore.ExistsAsync(courseName, cancellationToken);
    }

    public Task<Ulid?> IdentifyAsync(string courseName, CancellationToken cancellationToken = default)
    {
        return innerStore.IdentifyAsync(courseName, cancellationToken);
    }

    #region Create.

    public event EventHandler<CreatedEventArgs>? Created;

    public async Task CreateAsync(Course course, CancellationToken cancellationToken = default)
    {
        await innerStore.CreateAsync(course, cancellationToken).ConfigureAwait(false);
        Created?.Invoke(this, new CreatedEventArgs(course));
    }

    #endregion Create.

    public Task<ImmutableList<Course>> IndexAsync(CancellationToken cancellationToken = default)
    {
        return innerStore.IndexAsync(cancellationToken);
    }
}
