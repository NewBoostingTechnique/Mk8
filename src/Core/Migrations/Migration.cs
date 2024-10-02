namespace Mk8.Core.Migrations;

public record Migration
{
    public Ulid? Id { get; init; }

    public string? Description { get; init; }

    public byte Progress { get; init; }

    public string? Error { get; init; }

    public DateTime StartTime { get; init; }

    public DateTime? EndTime { get; init; }
}
