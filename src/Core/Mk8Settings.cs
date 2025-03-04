namespace Mk8.Core;

public record Mk8Settings
{
    public const string SectionName = "Mk8";

    public Uri ImportFromBaseUrl { get; init; } = new("https://mariokart64.com/mk8/");
}
