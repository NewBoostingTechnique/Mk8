namespace Mk8.Core.Players;

internal static class PlayerDataExtensions
{
    internal static async Task<Ulid> IdentifyRequiredAsync(
        this IPlayerStore playerData,
        string playerName,
        CancellationToken cancellationToken = default
    )
    {
        return await playerData.IdentifyAsync(playerName, cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException($"Player '{playerName}' not found.");
    }
}
