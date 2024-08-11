namespace Mk8.Core.Courses;

internal interface ICourseDataEvents
{

    #region Inserted.

    event EventHandler<InsertedEventArgs>? Inserted;

    internal sealed class InsertedEventArgs(Course course) : EventArgs
    {
        internal Course Course { get; } = course;
    }

    #endregion Inserted.

}
