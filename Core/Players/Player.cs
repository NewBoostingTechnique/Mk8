using System.Collections.Immutable;
using Mk8.Core.Times;

namespace Mk8.Core.Players;

public class Player
{
    internal string? Id { get; set; }

    public string? Name { get; set; }

    public DateOnly? Active { get; init; }

    #region Location.

    #region Country.

    internal string? CountryId { get; set; }

    public string? CountryName { get; set; }

    #endregion Country.

    #region Region.

    internal string? RegionId { get; set; }

    public string? RegionName { get; set; }

    #endregion Region.

    #endregion Location.

    #region ProofType.

    public string? ProofTypeId { get; set; }

    public string? ProofTypeDescription { get; set; }

    #endregion ProofType.

    public ImmutableList<Time> Times { get; set; } = [];
}
