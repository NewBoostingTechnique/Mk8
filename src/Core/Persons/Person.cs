namespace Mk8.Core.Persons;

public class Person
{
    public Ulid? Id { get; set; }

    public required string Name { get; init; }
}
