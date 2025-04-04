using System.Collections.Immutable;
using Mk8.Core.Persons;
using Mk8.Core.Times;

namespace Mk8.Core.Players;

public record Player : Person
{

    public DateOnly? Active { get; init; }

    #region Location.

    #region Country.

    public Ulid? CountryId { get; init; }

    public string? CountryName { get; init; }

    #endregion Country.

    #region Region.

    public Ulid? RegionId { get; init; }

    public string? RegionName { get; init; }

    #endregion Region.

    #endregion Location.

    public ImmutableList<Time> Times { get; init; } = [];
}
