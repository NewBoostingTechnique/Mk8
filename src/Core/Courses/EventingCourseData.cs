using System.Collections.Immutable;
using static Mk8.Core.Courses.ICourseDataEvents;

namespace Mk8.Core.Courses;

internal class EventingCourseData(ICourseData innerData)
    : ICourseData, ICourseDataEvents
{

    public Task<bool> ExistsAsync(string courseName)
    {
        return innerData.ExistsAsync(courseName);
    }

    public Task<Ulid?> IdentifyAsync(string courseName)
    {
        return innerData.IdentifyAsync(courseName);
    }

    #region Insert.

    public event EventHandler<InsertedEventArgs>? Inserted;

    public async Task CreateAsync(Course course)
    {
        await innerData.CreateAsync(course).ConfigureAwait(false);
        Inserted?.Invoke(this, new InsertedEventArgs(course));
    }

    #endregion Insert.

    public Task<IImmutableList<Course>> IndexAsync()
    {
        return innerData.IndexAsync();
    }
}
