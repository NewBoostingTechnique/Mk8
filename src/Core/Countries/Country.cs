namespace Mk8.Core.Countries;

public record Country
{
    public Ulid? Id { get; init; }

    public string? Name { get; init; }
}
