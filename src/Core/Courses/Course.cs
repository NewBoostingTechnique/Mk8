namespace Mk8.Core.Courses;

public record Course
{
    public Ulid? Id { get; internal init; }

    public required string Name { get; init; }
}
