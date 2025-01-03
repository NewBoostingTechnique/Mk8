namespace Mk8.Core.Courses;

public record Course
{
    public Ulid? Id { get; init; }

    public required string Name { get; init; }
}
