namespace Mk8.Core.Regions;

public record Region
{
    public Ulid? Id { get; init; }

    public string? Name { get; init; }

    public required Ulid CountryId { get; init; }
}
