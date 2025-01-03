using System.Diagnostics.CodeAnalysis;

namespace Mk8.Web.Times.Create;

public record CreateTimeRequest
{
    public DateOnly? Date { get; init; }

    public TimeSpan? Span { get; init; }

    public string? CourseName { get; init; }

    public string? PlayerName { get; init; }

    internal static readonly CreateTimeRequest Default = new();

    [MemberNotNullWhen(true, nameof(Date), nameof(Span), nameof(CourseName), nameof(PlayerName))]
    internal bool Validate([NotNullWhen(false)] out Dictionary<string, string[]>? errors)
    {
        Validator validator = new();
        validator.Require(Date);
        validator.Require(Span);
        validator.Require(CourseName);
        validator.Require(PlayerName);
        errors = validator.Errors;
        return errors is null;
    }
}
