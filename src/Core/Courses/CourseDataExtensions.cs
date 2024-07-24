namespace Mk8.Core.Courses;

internal static class CourseDataExtensions
{
    internal static async Task<string> IdentifyRequiredAsync(this ICourseData courseData, string courseName)
    {
        return await courseData.IdentifyAsync(courseName).ConfigureAwait(false)
            ?? throw new InvalidOperationException($"Course '{courseName}' not found.");
    }
}