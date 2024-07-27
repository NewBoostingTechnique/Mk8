namespace Mk8.Core.Times;

public class Time
{
    public Ulid? Id { get; internal set; }

    #region Course.

    public Ulid? CourseId { get; internal set; }

    public string? CourseName { get; set; }

    #endregion Course.

    public DateOnly? Date { get; set; }

    #region Player.

    public Ulid? PlayerId { get; internal set; }

    public string? PlayerName { get; set; }

    #endregion Player.

    public TimeSpan? Span { get; set; }
}
