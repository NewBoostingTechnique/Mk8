namespace Mk8.Core;

public class Mk8Settings
{
    public string? ConnectionString { get; init; }

    public Uri ImportFromBaseUrl { get; init; } = new("https://mariokart64.com/mk8/");

    public string? RootConnectionString { get; init; }
}
