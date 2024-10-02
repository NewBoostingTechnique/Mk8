namespace Mk8.Core.Persons;

public record Person
{
    public Ulid? Id { get; init; }

    public required string Name { get; init; }
}
