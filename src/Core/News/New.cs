namespace Mk8.Core.News;

public record New
{
    // TODO: All entities (records) should only have get, and init (no set).

    public Ulid? Id { get; internal init; }

    public required string Title { get; init; }

    public required DateOnly Date { get; init; }

    public required string Body { get; init; }

    public required string AuthorName { get; init; }

    public Ulid? AuthorPersonId { get; internal init; }
}
