namespace Mk8.Core.Times;

public record Time
{
    public Ulid? Id { get; internal init; }

    #region Course.

    public Ulid CourseId { get; init; }

    public string? CourseName { get; init; }

    #endregion Course.

    public DateOnly? Date { get; init; }

    #region Player.

    public Ulid PlayerId { get; init; }

    public string? PlayerName { get; init; }

    #endregion Player.

    public TimeSpan? Span { get; init; }
}
