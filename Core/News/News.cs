namespace Mk8.Core.News;

public class News
{
    public required string AuthorName { get; init; }

    public required string Body { get; init; }

    public required DateOnly Date { get; init; }

    public required string Title { get; init; }
}