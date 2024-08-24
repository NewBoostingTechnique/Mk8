namespace Mk8.Core.Players;

internal interface IPlayerSource
{
    IAsyncEnumerable<string> GetPlayersNamesAsync(CancellationToken cancellationToken = default);

    Task<string> GetRegionNameAsync(string playerName, CancellationToken cancellationToken = default);
}
