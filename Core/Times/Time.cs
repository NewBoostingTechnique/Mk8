namespace Mk8.Core.Times;

public class Time
{
    internal string? Id { get; set; }

    #region Course.

    internal string? CourseId { get; set; }

    public string? CourseName { get; set; }

    #endregion Course.

    public DateOnly? Date { get; set; }

    #region Player.

    internal string? PlayerId { get; set; }

    public string? PlayerName { get; set; }

    #endregion Player.

    public TimeSpan? Span { get; set; }
}
