namespace Mk8.Core;

public class Mk8Settings
{
    public string? ConnectionString { get; init; }

    public string? RootConnectionString { get; init; }

    public Uri ScrapeUrl { get; init; } = new("https://mariokart64.com/mk8/");
}
