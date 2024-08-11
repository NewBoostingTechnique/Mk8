namespace Mk8.Core.Locations.Regions;

public class Region
{
    public Ulid? Id { get; set; }

    public string? Name { get; set; }

    public required Ulid CountryId { get; set; }
}
