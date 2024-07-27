namespace Mk8.Core.News;

public class New
{
    public required string AuthorName { get; init; }

    public Ulid? AuthorPersonId { get; internal set; }

    public required string Body { get; init; }

    public required DateOnly Date { get; init; }

    public Ulid? Id { get; internal set; }

    public required string Title { get; init; }
}
