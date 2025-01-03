namespace Mk8.Core.Courses;

internal interface ICourseDataEvents
{

    #region Inserted.

    event EventHandler<CreatedEventArgs>? Created;

    internal sealed class CreatedEventArgs(Course course) : EventArgs
    {
        internal Course Course { get; } = course;
    }

    #endregion Inserted.

}
