namespace Mk8.Core.Times;

public class Time
{
    public string? Id { get; internal set; }

    #region Course.

    public string? CourseId { get; internal set; }

    public string? CourseName { get; set; }

    #endregion Course.

    public DateOnly? Date { get; set; }

    #region Player.

    public string? PlayerId { get; internal set; }

    public string? PlayerName { get; set; }

    #endregion Player.

    public TimeSpan? Span { get; set; }
}
