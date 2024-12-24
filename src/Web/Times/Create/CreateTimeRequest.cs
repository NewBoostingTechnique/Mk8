namespace Mk8.Web.Times.Create;

public record CreateTimeRequest
{
    public required DateOnly Date { get; init; }

    public required TimeSpan Span { get; init; }

    public required string CourseName { get; init; }

    public required string PlayerName { get; init; }
}
