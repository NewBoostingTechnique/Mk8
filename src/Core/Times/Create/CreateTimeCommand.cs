using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Mk8.Core.Times.Create;

public record CreateTimeCommand : ICommand<Result>
{
    public required DateOnly Date { get; init; }

    public required TimeSpan Span { get; init; }

    public required string CourseName { get; init; }

    public required string PlayerName { get; init; }
}
