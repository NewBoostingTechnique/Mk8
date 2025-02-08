namespace Mk8.Core;

public record Mk8Settings
{
    public Uri ImportFromBaseUrl { get; init; } = new("https://mariokart64.com/mk8/");
}
