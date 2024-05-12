namespace Mk8.Core.News;

public class New
{
    public required string AuthorName { get; init; }

    internal string? AuthorPersonId { get; set; }

    public required string Body { get; init; }

    public required DateOnly Date { get; init; }

    internal string? Id { get; set; }

    public required string Title { get; init; }
}