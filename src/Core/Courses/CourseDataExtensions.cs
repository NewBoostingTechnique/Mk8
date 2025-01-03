namespace Mk8.Core.Courses;

internal static class CourseDataExtensions
{
    internal static async Task<Ulid> IdentifyRequiredAsync(
        this ICourseStore courseData,
        string courseName,
        CancellationToken cancellationToken = default
    )
    {
        return await courseData.IdentifyAsync(courseName, cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException($"Course '{courseName}' not found.");
    }
}
