using System.Collections.Immutable;
using Mk8.Core.Persons;
using Mk8.Core.Times;

namespace Mk8.Core.Players;

public class Player : Person
{

    public DateOnly? Active { get; init; }

    #region Location.

    #region Country.

    public string? CountryId { get; internal set; }

    public string? CountryName { get; set; }

    #endregion Country.

    #region Region.

    public string? RegionId { get; internal set; }

    public string? RegionName { get; set; }

    #endregion Region.

    #endregion Location.

    #region ProofType.

    public string? ProofTypeId { get; set; }

    public string? ProofTypeDescription { get; set; }

    #endregion ProofType.

    public ImmutableList<Time> Times { get; set; } = [];
}
